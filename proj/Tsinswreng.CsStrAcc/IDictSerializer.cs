using System.Collections;
using System.Collections.Generic;
using Tsinswreng.CsCore;

namespace Tsinswreng.CsStrAcc;

public interface IDictSerializer{
	public obj? Serialize(obj? Obj, Type? Type = null);
	public obj? Deserialize(obj? Src, Type TargetType, obj? TargetObj = null);
}

public interface ITypeConverter{
	[Doc("For serialize")]
	public obj? Convert(obj? Obj, Type Type);
}

[Doc(@$"
deep serialize an object to a nested `IDictionary<str, obj?>` or `IList<obj?>`,
or deserialize from that to obj
")]
public class DictSerializer{
	public IDictionary<Type, ITypeConverter> Converters{get;set;}
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
		PropAccessors = new Dictionary<Type, IPropAccessor>();
		InstMkrs = new Dictionary<Type, IInstMkr>();
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
		if(Src is null){
			return null;
		}

		if(TargetType == typeof(obj)){
			return Src;
		}

		if(Converters.TryGetValue(TargetType, out var Convtr)){
			return Convtr.Convert(Src, TargetType);
		}

		if(IsPrimitiveType(TargetType)){
			return ConvertPrimitive(Src, TargetType);
		}

		if(TargetType.IsEnum){
			if(Src is str EnumName){
				return Enum.Parse(TargetType, EnumName, true);
			}
			return Enum.ToObject(TargetType, Src);
		}

		if(TargetType.IsAssignableFrom(Src.GetType())){
			return Src;
		}

		if(Src is IDictionary SrcDict){
			if(typeof(IDictionary).IsAssignableFrom(TargetType)){
				if(TargetObj is IDictionary TarDict){
					FillDictionaryRaw(SrcDict, TarDict);
					return TarDict;
				}
				var NewDict = new Dictionary<str, obj?>();
				FillDictionaryRaw(SrcDict, NewDict);
				return NewDict;
			}

			return DeserializeObjectFromDict(SrcDict, TargetType, TargetObj);
		}

		if(Src is IEnumerable SrcList && Src is not str){
			if(TargetObj is IList TarList){
				FillList(SrcList, TargetType, TarList);
				return TarList;
			}
			if(typeof(IList).IsAssignableFrom(TargetType)){
				var EleType = GetListElementType(TargetType) ?? typeof(obj);
				if(!InstMkrs.TryGetValue(EleType, out var Mkr)){
					throw new NotSupportedException($"No {nameof(IInstMkr)} for list element type {EleType}");
				}
				var NewList = Mkr.MkList();
				if(NewList is not IList List){
					throw new NotSupportedException($"{nameof(IInstMkr)}.{nameof(IInstMkr.MkList)}() must return {nameof(IList)}");
				}
				FillList(SrcList, TargetType, List);
				return List;
			}
		}

		throw new NotSupportedException($"Cannot deserialize from {Src.GetType()} to {TargetType}");
	}

	protected virtual obj? DeserializeObjectFromDict(IDictionary SrcDict, Type TargetType, obj? TargetObj){
		if(!PropAccessors.TryGetValue(TargetType, out var PropAcc)){
			throw new NotSupportedException($"No prop accessor for type {TargetType}");
		}

		var TarObj = TargetObj;
		if(TarObj is null){
			if(!InstMkrs.TryGetValue(TargetType, out var Mkr)){
				throw new NotSupportedException($"No {nameof(IInstMkr)} for type {TargetType}");
			}
			TarObj = Mkr.MkInst();
		}

		foreach(DictionaryEntry Kv in SrcDict){
			if(Kv.Key is not str Key){
				continue;
			}
			if(!PropAcc.TryGetType(Key, out var PropType) || PropType is null){
				continue;
			}
			var DeVal = Deserialize(Kv.Value, PropType);
			PropAcc.TrySet(TarObj, Key, DeVal);
		}
		return TarObj;
	}

	protected virtual void FillDictionaryRaw(IDictionary Src, IDictionary Tar){
		foreach(DictionaryEntry Kv in Src){
			Tar[Kv.Key] = Kv.Value;
		}
	}

	protected virtual void FillList(IEnumerable SrcList, Type TargetListType, IList TarList){
		var EleType = GetListElementType(TargetListType) ?? typeof(obj);
		foreach(var Ele in SrcList){
			TarList.Add(Deserialize(Ele, EleType));
		}
	}

	protected virtual obj? ConvertPrimitive(obj Src, Type TargetType){
		if(TargetType.IsAssignableFrom(Src.GetType())){
			return Src;
		}
		if(TargetType == typeof(string)){
			return Src.ToString();
		}
		if(TargetType == typeof(Guid)){
			if(Src is Guid G){
				return G;
			}
			if(Src is str S){
				return Guid.Parse(S);
			}
		}
		return Convert.ChangeType(Src, TargetType);
	}
}
