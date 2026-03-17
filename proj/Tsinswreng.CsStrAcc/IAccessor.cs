using System;
using System.Collections.Generic;
using Tsinswreng.CsCore;
namespace Tsinswreng.CsStrAcc;

[Doc(@$"Access props by string
only support props(getter, setter, etc.)
functions are not included currently because they may have overloads
")]
public interface IPropAccessor{
	public bool TryGet(obj O, str Key, out obj? R);
	[Doc(@$"
	#Rtn[true if ok]")]
	public bool TrySet(obj O, str Key, obj? Value);
	
	[Doc(@$"
	get properties of object(getter, setter etc)
	raw fields, methods are not included
	#Params([],[Option, not supported yet, reserved for future use])
	#Rtn[Read only, thus in every call, address of returned list may not change ]")]
	public IReadOnlyList<str> GetProps(obj O, OptGetProps? Opt = null);
	[Doc(@$"Get Declared type of prop
	NOT the same as below:
	```cs
	{nameof(TryGet)}(o, key, out var v);
	var GotType = v.GetType();
	```
	+ out var v may be null, if null, you won't get the type
	+ GetType() returns actual type, not declared type
	")]
	public bool TryGetType(obj O, str Key, out Type? Type);
}

[Doc(@$"no content, reserved for future use")]
public class OptGetProps{
	
}

[Doc(@$"no content, reserved for future use")]
public class OptGetKeys{
	
}
