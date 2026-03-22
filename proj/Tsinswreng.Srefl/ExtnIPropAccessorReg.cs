using System;
using System.Collections.Generic;
using System.Linq;

namespace Tsinswreng.Srefl;

public static class ExtnIPropAccessorReg{
	extension(IPropAccessorReg z){
		public bool TryGet(obj? O, Type Target, str Key, out obj? R){
			R = null;
			if(!z.Type_PropAccessor.TryGetValue(Target, out var accessor)){
				return false;
			}
			return accessor.TryGet(O, Key, out R);
		}

		public bool TrySet(obj? O, Type Target, str Key, obj? Value){
			if(!z.Type_PropAccessor.TryGetValue(Target, out var accessor)){
				return false;
			}
			return accessor.TrySet(O, Key, Value);
		}

		public IReadOnlyList<str> GetProps(Type Target, OptGetGetterNames? Opt = null){
			if(!z.Type_PropAccessor.TryGetValue(Target, out var accessor)){
				return Array.Empty<str>();
			}
			return accessor.GetGetterNames(null, Opt).ToArray();
		}

		public bool TryGetType(Type Target, str Key, out Type? Type){
			Type = null;
			if(!z.Type_PropAccessor.TryGetValue(Target, out var accessor)){
				return false;
			}
			return accessor.TryGetType(Key, out Type);
		}
		
		public IPropDict ToPropDict(obj? O, Type Target){
			var accessor = z.Type_PropAccessor[Target];
			return new PropDict(accessor, O);
		}
		public IPropDict ToPropDict<T>(T O){
			var Target = typeof(T);
			var accessor = z.Type_PropAccessor[Target];
			return new PropDict(accessor, O);
		}
	}
}
