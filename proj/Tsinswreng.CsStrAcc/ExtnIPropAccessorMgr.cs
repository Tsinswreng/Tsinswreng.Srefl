using System;
using System.Collections.Generic;
using System.Linq;

namespace Tsinswreng.CsStrAcc;

public static class ExtnIPropAccessorMgr{
	extension(IPropAccessorMgr z){
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
	}
}
