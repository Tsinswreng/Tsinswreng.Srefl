using System.Runtime.CompilerServices;
using Tsinswreng.Srefl.Test.Models;

namespace Tsinswreng.Srefl.Test;

/// 這是按 SharedModels 裏的 GeneralNsDerivedModel 寫出的「一份具體輸出樣例」。
/// 用來直觀看源生成後大概會是什麼結構（這個類本身不參與測試註冊）。
public sealed class SrcGenOutputSample_GeneralNsDerivedModel: IPropAccessor{
	public Type TargetType { get; } = typeof(GeneralNsDerivedModel);

	private static readonly string[] _GetterNames = [
		nameof(GeneralNsDerivedModel.Age),
		nameof(GeneralNsDerivedModel.Name),
		nameof(GeneralNsDerivedModel.Score),
		nameof(GeneralNsBaseModel.BaseId),
		"PrivateProp",
		"PrivateBaseProp",
		nameof(GeneralNsBaseModel.ReadOnlyBaseProp),
		nameof(GeneralNsBaseModel.WriteOnlySink),
	];

	private static readonly string[] _SetterNames = [
		nameof(GeneralNsDerivedModel.Age),
		nameof(GeneralNsDerivedModel.Name),
		nameof(GeneralNsDerivedModel.Score),
		nameof(GeneralNsBaseModel.BaseId),
		"PrivateProp",
		"PrivateBaseProp",
		nameof(GeneralNsBaseModel.WriteOnlyBaseProp),
	];

	public bool TryGet(object? O, string Key, out object? R){
		R = null;
		if(O is not GeneralNsDerivedModel o){
			return false;
		}
		switch(Key){
			case nameof(GeneralNsDerivedModel.Age): R = o.Age; return true;
			case nameof(GeneralNsDerivedModel.Name): R = o.Name; return true;
			case nameof(GeneralNsDerivedModel.Score): R = o.Score; return true;
			case nameof(GeneralNsBaseModel.BaseId): R = o.BaseId; return true;
			case nameof(GeneralNsBaseModel.ReadOnlyBaseProp): R = o.ReadOnlyBaseProp; return true;
			case nameof(GeneralNsBaseModel.WriteOnlySink): R = o.WriteOnlySink; return true;
			case "PrivateProp": R = __UnsafeGetPrivateProp(o); return true;
			case "PrivateBaseProp": R = __UnsafeGetPrivateBaseProp(o); return true;
			default: return false;
		}
	}

	public bool TrySet(object? O, string Key, object? Value){
		if(O is not GeneralNsDerivedModel o){
			return false;
		}
		switch(Key){
			case nameof(GeneralNsDerivedModel.Age): o.Age = (int)Value!; return true;
			case nameof(GeneralNsDerivedModel.Name): o.Name = (string?)Value; return true;
			case nameof(GeneralNsDerivedModel.Score): o.Score = (decimal)Value!; return true;
			case nameof(GeneralNsBaseModel.BaseId): o.BaseId = (int)Value!; return true;
			case nameof(GeneralNsBaseModel.WriteOnlyBaseProp): o.WriteOnlyBaseProp = (int)Value!; return true;
			case "PrivateProp": __UnsafeSetPrivateProp(o, (string?)Value); return true;
			case "PrivateBaseProp": __UnsafeSetPrivateBaseProp(o, (int)Value!); return true;
			default: return false;
		}
	}

	public IReadOnlyCollection<string> GetGetterNames(object? O, OptGetGetterNames? Opt = null){
		return _GetterNames;
	}

	public IReadOnlyCollection<string> GetSetterNames(object? O, OptGetSetterNames? Opt = null){
		return _SetterNames;
	}

	public bool TryGetType(string Key, out Type? Type){
		Type = null;
		switch(Key){
			case nameof(GeneralNsDerivedModel.Age): Type = typeof(int); return true;
			case nameof(GeneralNsDerivedModel.Name): Type = typeof(string); return true;
			case nameof(GeneralNsDerivedModel.Score): Type = typeof(decimal); return true;
			case nameof(GeneralNsBaseModel.BaseId): Type = typeof(int); return true;
			default: return false;
		}
	}

	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_PrivateProp")]
	private static extern string? __UnsafeGetPrivateProp(GeneralNsDerivedModel O);

	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "set_PrivateProp")]
	private static extern void __UnsafeSetPrivateProp(GeneralNsDerivedModel O, string? Value);

	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_PrivateBaseProp")]
	private static extern int __UnsafeGetPrivateBaseProp(GeneralNsBaseModel O);

	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "set_PrivateBaseProp")]
	private static extern void __UnsafeSetPrivateBaseProp(GeneralNsBaseModel O, int Value);
}
