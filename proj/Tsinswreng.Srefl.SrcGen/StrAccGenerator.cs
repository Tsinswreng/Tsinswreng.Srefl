using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Tsinswreng.Srefl.SrcGen;

[Generator]
public sealed class StrAccGenerator: ISourceGenerator{
	public void Initialize(GeneratorInitializationContext context){
	}

	public void Execute(GeneratorExecutionContext context){
		try{
			var allTypes = context.Compilation.SourceModule.GlobalNamespace.GetAllNamedTypes();
			foreach(var hostType in allTypes){
				var targetTypes = GetRegisteredTargetTypes(hostType).ToArray();
				if(targetTypes.Length == 0){
					continue;
				}
				var code = BuildHostPartial(hostType, targetTypes);
				context.AddSource($"{hostType.ToDisplayString().Replace('<','_').Replace('>','_').Replace('.', '_')}.StrAcc.g.cs", code);
			}
		}
		catch(Exception e){
			throw new Exception($"{nameof(StrAccGenerator)} failed: {e}", e);
		}
	}

	private static IEnumerable<INamedTypeSymbol> GetRegisteredTargetTypes(INamedTypeSymbol hostType){
		var attrs = hostType.GetAttributes()
			.Where(a => a.AttributeClass?.Name == nameof(StrAccType) || a.AttributeClass?.Name == nameof(StrAccType) + "Attribute");

		var seen = new HashSet<string>(StringComparer.Ordinal);
		foreach(var attr in attrs){
			var arg = attr.ConstructorArguments.FirstOrDefault();
			if(arg.Value is not INamedTypeSymbol targetType){
				continue;
			}
			var key = targetType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
			if(seen.Add(key)){
				yield return targetType;
			}
		}
	}

	private static string BuildHostPartial(INamedTypeSymbol hostType, IReadOnlyList<INamedTypeSymbol> targetTypes){
		var n = new ConstName();
		var sb = new StringBuilder();
		sb.AppendLine("#pragma warning disable CS8618, CS8600, CS8601, CS8602, CS8603, CS8604, CS8605");
		sb.AppendLine("using System;");
		sb.AppendLine("using System.Collections.Generic;");
		sb.AppendLine("using System.Linq;");
		sb.AppendLine("using System.Runtime.CompilerServices;");

		var ns = hostType.ContainingNamespace?.IsGlobalNamespace == false
			? hostType.ContainingNamespace.ToDisplayString()
			: "";
		if(!string.IsNullOrWhiteSpace(ns)){
			sb.AppendLine($"namespace {ns}{{");
		}

		AppendContainingTypesOpen(sb, hostType);
		sb.AppendLine($"public partial class {hostType.Name}: global::{n.NsDictMapper}.IPropAccessorReg{{");
		sb.AppendLine("\tpublic global::Tsinswreng.Srefl.IPropAccessorReg PropAccessorMgr { get; set; }");
		sb.AppendLine($"\tpublic {hostType.Name}(){{");
		sb.AppendLine("\t\tthis.PropAccessorMgr = new __GeneratedPropAccessorMgr();");
		sb.AppendLine("\t}");
		// 兼容旧代码：一些地方可能直接把 ctx 当作 mgr 使用
		sb.AppendLine("\tpublic global::System.Collections.Generic.IDictionary<global::System.Type, global::Tsinswreng.Srefl.IPropAccessor> Type_PropAccessor {");
		sb.AppendLine("\t\tget => this.PropAccessorMgr.Type_PropAccessor;");
		sb.AppendLine("\t\tset => this.PropAccessorMgr.Type_PropAccessor = value;");
		sb.AppendLine("\t}");

		AppendMgrClass(sb, targetTypes);
		for(var i = 0; i < targetTypes.Count; i++){
			AppendAccessorClass(sb, targetTypes[i], i);
		}

		sb.AppendLine("}");
		AppendContainingTypesClose(sb, hostType);
		if(!string.IsNullOrWhiteSpace(ns)){
			sb.AppendLine("}");
		}
		return sb.ToString();
	}

	private static void AppendMgrClass(StringBuilder sb, IReadOnlyList<INamedTypeSymbol> targetTypes){
		sb.AppendLine("\tprivate sealed class __GeneratedPropAccessorMgr: global::Tsinswreng.Srefl.IPropAccessorReg{");
		sb.AppendLine("\t\tpublic global::System.Collections.Generic.IDictionary<global::System.Type, global::Tsinswreng.Srefl.IPropAccessor> Type_PropAccessor { get; set; }");
		sb.AppendLine("\t\tpublic __GeneratedPropAccessorMgr(){");
		sb.AppendLine("\t\t\tType_PropAccessor = new global::System.Collections.Generic.Dictionary<global::System.Type, global::Tsinswreng.Srefl.IPropAccessor>{");
		for(var i = 0; i < targetTypes.Count; i++){
			var typeExpr = CodeTool.ResolveFullTypeFitsTypeof(targetTypes[i]);
			sb.AppendLine($"\t\t\t\t[typeof({typeExpr})] = __GeneratedPropAccessor_{i}.Inst,");
		}
		sb.AppendLine("\t\t\t};");
		sb.AppendLine("\t\t}");
		sb.AppendLine("\t}");
	}

	private static void AppendAccessorClass(StringBuilder sb, INamedTypeSymbol targetType, int index){
		var typeExpr = CodeTool.ResolveFullTypeFitsTypeof(targetType);
		var props = CollectUniqueProperties(targetType);
		var getterProps = props.Where(p => p.GetMethod is not null).ToArray();
		var setterProps = props.Where(p => p.SetMethod is not null).ToArray();
		var publicForType = props.Where(p => p.DeclaredAccessibility == Accessibility.Public).ToArray();

		sb.AppendLine($"\tprivate sealed class __GeneratedPropAccessor_{index}: global::Tsinswreng.Srefl.IPropAccessor{{");
		sb.AppendLine($"\t\tpublic static readonly __GeneratedPropAccessor_{index} Inst = new __GeneratedPropAccessor_{index}();");
		sb.AppendLine($"\t\tpublic Type TargetType {{ get; }} = typeof({typeExpr});");
		sb.AppendLine("\t\tprivate static readonly string[] __GetterNames = new string[]{");
		foreach(var p in getterProps){
			sb.AppendLine($"\t\t\t\"{p.Name}\",");
		}
		sb.AppendLine("\t\t};");
		sb.AppendLine("\t\tprivate static readonly string[] __SetterNames = new string[]{");
		foreach(var p in setterProps){
			sb.AppendLine($"\t\t\t\"{p.Name}\",");
		}
		sb.AppendLine("\t\t};");

		var getUnsafeDecls = new StringBuilder();
		var setUnsafeDecls = new StringBuilder();

		sb.AppendLine("\t\tpublic bool TryGet(object? O, string Key, out object? R){");
		sb.AppendLine("\t\t\tR = null;");
		sb.AppendLine($"\t\t\tif(O is not {typeExpr} o){{ return false; }}");
		sb.AppendLine("\t\t\tswitch(Key){");
		var iGet = 0;
		foreach(var p in getterProps){
			var isPublic = p.GetMethod?.DeclaredAccessibility == Accessibility.Public;
			sb.AppendLine($"\t\t\t\tcase \"{p.Name}\":");
			if(isPublic){
				sb.AppendLine($"\t\t\t\t\tR = o.{p.Name};");
			}
			else{
				var declType = CodeTool.ResolveFullTypeFitsTypeof(p.ContainingType!);
				var propType = p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
				var fn = $"__UnsafeGet_{iGet}";
				getUnsafeDecls.AppendLine($"\t\t[UnsafeAccessor(UnsafeAccessorKind.Method, Name = \"get_{p.Name}\")]");
				getUnsafeDecls.AppendLine($"\t\tprivate static extern {propType} {fn}({declType} O);");
				sb.AppendLine($"\t\t\t\t\tR = {fn}(({declType})o);");
			}
			sb.AppendLine("\t\t\t\t\treturn true;");
			iGet++;
		}
		sb.AppendLine("\t\t\t\tdefault:");
		sb.AppendLine("\t\t\t\t\treturn false;");
		sb.AppendLine("\t\t\t}");
		sb.AppendLine("\t\t}");

		sb.AppendLine("\t\tpublic bool TrySet(object? O, string Key, object? Value){");
		sb.AppendLine($"\t\t\tif(O is not {typeExpr} o){{ return false; }}");
		sb.AppendLine("\t\t\tswitch(Key){");
		var iSet = 0;
		foreach(var p in setterProps){
			var isPublic = p.SetMethod?.DeclaredAccessibility == Accessibility.Public;
			var propType = p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
			sb.AppendLine($"\t\t\t\tcase \"{p.Name}\":");
			if(isPublic){
				sb.AppendLine($"\t\t\t\t\to.{p.Name} = ({propType})Value!;");
			}
			else{
				var declType = CodeTool.ResolveFullTypeFitsTypeof(p.ContainingType!);
				var fn = $"__UnsafeSet_{iSet}";
				setUnsafeDecls.AppendLine($"\t\t[UnsafeAccessor(UnsafeAccessorKind.Method, Name = \"set_{p.Name}\")]");
				setUnsafeDecls.AppendLine($"\t\tprivate static extern void {fn}({declType} O, {propType} Value);");
				sb.AppendLine($"\t\t\t\t\t{fn}(({declType})o, ({propType})Value!);");
			}
			sb.AppendLine("\t\t\t\t\treturn true;");
			iSet++;
		}
		sb.AppendLine("\t\t\t\tdefault:");
		sb.AppendLine("\t\t\t\t\treturn false;");
		sb.AppendLine("\t\t\t}");
		sb.AppendLine("\t\t}");

		sb.AppendLine("\t\tpublic IReadOnlyCollection<string> GetGetterNames(object? O, global::Tsinswreng.Srefl.OptGetGetterNames? Opt = null){");
		sb.AppendLine("\t\t\treturn __GetterNames;");
		sb.AppendLine("\t\t}");
		sb.AppendLine("\t\tpublic IReadOnlyCollection<string> GetSetterNames(object? O, global::Tsinswreng.Srefl.OptGetSetterNames? Opt = null){");
		sb.AppendLine("\t\t\treturn __SetterNames;");
		sb.AppendLine("\t\t}");
		sb.AppendLine("\t\tpublic bool TryGetType(string Key, out Type? Type){");
		sb.AppendLine("\t\t\tType = null;");
		sb.AppendLine("\t\t\tswitch(Key){");
		foreach(var p in publicForType){
			var propType = CodeTool.ResolveFullTypeFitsTypeof(p.Type);
			sb.AppendLine($"\t\t\t\tcase \"{p.Name}\": Type = typeof({propType}); return true;");
		}
		sb.AppendLine("\t\t\t\tdefault: return false;");
		sb.AppendLine("\t\t\t}");
		sb.AppendLine("\t\t}");

		sb.Append(getUnsafeDecls.ToString());
		sb.Append(setUnsafeDecls.ToString());
		sb.AppendLine("\t}");
	}

	private static IReadOnlyList<IPropertySymbol> CollectUniqueProperties(INamedTypeSymbol targetType){
		var result = new List<IPropertySymbol>();
		var seen = new HashSet<string>(StringComparer.Ordinal);
		for(INamedTypeSymbol? t = targetType; t is not null; t = t.BaseType){
			foreach(var p in t.GetMembers().OfType<IPropertySymbol>()){
				if(p.IsStatic || p.IsIndexer){
					continue;
				}
				if(seen.Add(p.Name)){
					result.Add(p);
				}
			}
		}
		return result;
	}

	private static void AppendContainingTypesOpen(StringBuilder sb, INamedTypeSymbol hostType){
		var stack = new Stack<INamedTypeSymbol>();
		for(var t = hostType.ContainingType; t is not null; t = t.ContainingType){
			stack.Push(t);
		}
		while(stack.Count > 0){
			var t = stack.Pop();
			sb.AppendLine($"partial class {t.Name}{{");
		}
	}

	private static void AppendContainingTypesClose(StringBuilder sb, INamedTypeSymbol hostType){
		for(var t = hostType.ContainingType; t is not null; t = t.ContainingType){
			sb.AppendLine("}");
		}
	}
}
