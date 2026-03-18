using System;
using System.Collections;
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
	#Rtn[Read only, thus in every call, address of returned obj may not change ]
	")]
	public IReadOnlyCollection<string> GetPropNames(obj? O, OptGetPropNames? Opt = null);
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

//TODO
//public class PropDict : IDictionary<str, obj?> {
// 	IPropAccessor PropAcc;
// 	Type TargetType;
// 	obj? TargetObj;
// 	public object? this[string key] {
// 		get=>
// 	}

// 	public ICollection<string> Keys => throw new NotImplementedException();

// 	public ICollection<object?> Values => throw new NotImplementedException();

// 	public int Count => throw new NotImplementedException();

// 	public bool IsReadOnly => throw new NotImplementedException();

// 	public void Add(string key, object? value) {
// 		throw new NotImplementedException();
// 	}

// 	public void Add(KeyValuePair<string, object?> item) {
// 		throw new NotImplementedException();
// 	}

// 	public void Clear() {
// 		throw new NotImplementedException();
// 	}

// 	public bool Contains(KeyValuePair<string, object?> item) {
// 		throw new NotImplementedException();
// 	}

// 	public bool ContainsKey(string key) {
// 		throw new NotImplementedException();
// 	}

// 	public void CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex) {
// 		throw new NotImplementedException();
// 	}

// 	public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() {
// 		throw new NotImplementedException();
// 	}

// 	public bool Remove(string key) {
// 		throw new NotImplementedException();
// 	}

// 	public bool Remove(KeyValuePair<string, object?> item) {
// 		throw new NotImplementedException();
// 	}

// 	public bool TryGetValue(string key, out object? value) {
// 		throw new NotImplementedException();
// 	}

// 	IEnumerator IEnumerable.GetEnumerator() {
// 		return GetEnumerator();
// 	}
// }

[Doc(@$"no content, reserved for future use")]
public class OptGetPropNames{
	
}




