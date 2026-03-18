using System;
using System.Collections;
using System.Collections.Generic;
using Tsinswreng.CsCore;
namespace Tsinswreng.CsStrAcc;

[Doc(@$"Access props by string
defaultly support both public and non-public getters and setters.
functions are not included currently because they may have overloads.
")]
public interface IPropAccessor{
	public Type TargetType{get;}
	[Doc(@$"
	support public and non-public getter
	#Params([its type should be assignable to `{nameof(TargetType)}`])
	")]
	public bool TryGet(obj? O, str Key, out obj? R);
	[Doc(@$"
	support public and non-public setter
	#Params([its type should be assignable to `{nameof(TargetType)}`])
	#Rtn[true if ok]")]
	public bool TrySet(obj? O, str Key, obj? Value);
	[Doc(@$"
	support public and non-public getter
	#Params(
	[its type should be assignable to `{nameof(TargetType)}`],
	[Option, not supported yet, reserved for future use],
	)
	#Rtn[Read only, thus in every call, address of returned obj may not change.
	]
	")]
	public IReadOnlyCollection<string> GetGetterNames(obj? O, OptGetGetterNames? Opt = null);
	[Doc(@$"
	support public and non-public setter
	#Params(
	[its type should be assignable to `{nameof(TargetType)}`],
	[Option, not supported yet, reserved for future use],
	)
	#Rtn[Read only, thus in every call, address of returned obj may not change.
	]
	")]
	public IReadOnlyCollection<string> GetSetterNames(obj? O, OptGetSetterNames? Opt = null);
	
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
public class OptGetGetterNames{
	
}

[Doc(@$"no content, reserved for future use")]
public class OptGetSetterNames{
	
}
