using System.Collections;
using System.Collections.Generic;
using Tsinswreng.CsCore;

namespace Tsinswreng.CsStrAcc;


public interface ITypeConverter{
	[Doc("For serialize")]
	public obj? Convert(obj? Obj, Type Type);
}

public interface ITypeDeConverter{
	[Doc("For deserialize")]
	public obj? Convert(obj? Src, Type TargetType);
}

[Doc(@$"
deep serialize an object to a nested `IDictionary<str, obj?>` or `IList<obj?>`,
or deserialize from that to obj.

AOT safe.

support:
- config what type is primitive type
- config how to convert a value of non-primitive type when serialize/deserialize
note: type mapping is not one-to-one
e.g when serialize, there may be more than one type mapped to `string`,
")]
public class DictSerializer{
	public IDictionary<Type, ITypeConverter> Converters{get;set;}
	public IDictionary<Type, ITypeDeConverter> DeConverters{get;set;}
	public IDictionary<Type, IPropAccessor> PropAccessors{get;set;}
	
	[Doc(@$"
	Create instance/list without runtime reflection.
	key = element/object target type.
	")]
	public IDictionary<Type, IInstMkr> InstMkrs{get;set;}
	
	
	public Func<Type, bool> IsPrimitiveType{get;set;} = (Type)=>{
		return Type.IsPrimitive || Type == typeof(string);
	};

	[Doc(@$"
	AOT-safe list element type resolver.
	Should be provided by caller/source-generated mapping.
	If returns null, list element will fallback to `obj`.
	")]
	public Func<Type, Type?> GetListElementType{get;set;} = (_)=>null;

	public DictSerializer(){
		Converters = new Dictionary<Type, ITypeConverter>();
		DeConverters = new Dictionary<Type, ITypeDeConverter>();
		PropAccessors = new Dictionary<Type, IPropAccessor>();
		InstMkrs = new Dictionary<Type, IInstMkr>();
	}

	public void SetConverter(Type Type, ITypeConverter Converter){
		Converters[Type] = Converter;
	}

	public void SetDeConverter(Type Type, ITypeDeConverter Converter){
		DeConverters[Type] = Converter;
	}
	
	[Doc(@$"
	deep serialize an object to a nested `IDictionary<str, obj?>` or `IList<obj?>`
	#Params([],[if null, use `Obj.GetType()`])
	#Rtn[
	nested `IDictionary<str, obj?>` or IList<obj?>
	]
	")]
	public obj? Serialize(
		obj? Obj, Type? Type = null
	){
		if(Obj is null){
			return null;
		}
		Type??= Obj.GetType();
		if(Converters.TryGetValue(Type, out var Convtr)){
			return Convtr.Convert(Obj, Type);
		}
		if(IsPrimitiveType(Type)){
			return Obj;
		}
		{
			if(Obj is IEnumerable list){
				var R = new List<obj?>();
				foreach(var ele in list){
					R.Add(Serialize(ele));
				}
				return R;
			}
		}
		{
			if(!PropAccessors.TryGetValue(Type, out var PropAcc)){
				throw new NotSupportedException($"No prop accessor for type {Type}");
			}
			var R = new Dictionary<str, obj?>();
			foreach(var k in PropAcc.GetGetterNames(Obj)){
				if(PropAcc.TryGet(Obj, k, out var v)){
					R[k] = Serialize(v);
				}
			}
			return R;
		}
	}

	[Doc(@$"
	deep deserialize from nested `IDictionary<str, obj?>` or `IList<obj?>`
	to target object.
	AOT-safe: no `Activator.CreateInstance`, no `MakeGenericType`.
	")]
	public obj? Deserialize(obj? Src, Type TargetType, obj? TargetObj = null){
		throw new NotImplementedException();
	}

}
