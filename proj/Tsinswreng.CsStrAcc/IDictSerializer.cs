using System.Collections;

namespace Tsinswreng.CsStrAcc;

public interface IDictSerializer{
	
}

public interface ITypeConverter{
	public obj? Convert(obj? Obj, Type Type);
}

public class DictSerializer{
	public IDictionary<Type, ITypeConverter> Converters{get;set;}
	public IDictionary<Type, IPropAccessor> PropAccessors{get;set;}
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
		
		if(Type.IsPrimitive || Type == typeof(string)){
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
	
}
