using System;
using System.Collections.Generic;
using Tsinswreng.CsCore;
namespace Tsinswreng.CsStrAcc;


[Doc(@$"Access props by string
only support props(getter, setter, etc.)
functions are not included currently because they may have overloads
")]
public interface IPropAccessor{
	public Type TargetType{get;}
	[Doc(@$"
	#Params([its type should be assignable to `{nameof(TargetType)}`])
	")]
	public bool TryGet(obj? O, str Key, out obj? R);
	[Doc(@$"
	#Params([its type should be assignable to `{nameof(TargetType)}`])
	#Rtn[true if ok]")]
	public bool TrySet(obj? O, str Key, obj? Value);
	[Doc(@$"
	#Params(
	[its type should be assignable to `{nameof(TargetType)}`],
	[Option, not supported yet, reserved for future use],
	)
	#Rtn[Read only, thus in every call, address of returned list may not change ]
	")]
	public IReadOnlyList<str> GetPropNames(obj? O, OptGetPropNames? Opt = null);
	[Doc(@$"
	Get Declared type of {nameof(TargetType)}'s Member at {nameof(Key)}
	NOT the same as below:
	```cs
	{nameof(TryGet)}(o, key, out var v);
	var GotType = v.GetType();
	```
	+ out var v may be null, if null, you won't get the type
	+ GetType() returns actual type, not declared type
	")]
	
	public bool TryGetType(str Key, out Type? Type);
}

[Doc(@$"no content, reserved for future use")]
public class OptGetPropNames{
	
}


public interface IPropAccessorMgr{
	public IDictionary<Type, IPropAccessor> Type_PropAccessor{get;set;}
	#region 下面這些放到IPropAccessorMgr的擴展方法裏去
	public bool TryGet(obj? O, Type Target, str Key, out obj? R);

	public bool TrySet(obj? O, Type Target, str Key, obj? Value);
	
	public IReadOnlyList<str> GetProps(Type Target, OptGetPropNames? Opt = null);
	
	public bool TryGetType(Type Target, str Key, out Type? Type);
	#endregion
	
}

