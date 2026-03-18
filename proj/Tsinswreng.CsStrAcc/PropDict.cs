using Tsinswreng.CsCore;

namespace Tsinswreng.CsStrAcc;
public interface IPropDict:IDictionary<str, obj?>{
	public IPropAccessor PropAccessor{get;set;}
	public Type TargetType{get;set;}
	
	[Doc(@$"Should be assignable to {nameof(TargetType)}")]
	public obj? TargetObj{get;set;}
}
public class PropDict : IPropDict {
	
	public object? this[string key] {
		get=>
	}

	public ICollection<string> Keys => throw new NotImplementedException();

	public ICollection<object?> Values => throw new NotImplementedException();

	public int Count => throw new NotImplementedException();

	public bool IsReadOnly => throw new NotImplementedException();

	public void Add(string key, object? value) {
		throw new NotImplementedException();
	}

	public void Add(KeyValuePair<string, object?> item) {
		throw new NotImplementedException();
	}

	public void Clear() {
		throw new NotImplementedException();
	}

	public bool Contains(KeyValuePair<string, object?> item) {
		throw new NotImplementedException();
	}

	public bool ContainsKey(string key) {
		throw new NotImplementedException();
	}

	public void CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex) {
		throw new NotImplementedException();
	}

	public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() {
		throw new NotImplementedException();
	}

	public bool Remove(string key) {
		throw new NotImplementedException();
	}

	public bool Remove(KeyValuePair<string, object?> item) {
		throw new NotImplementedException();
	}

	public bool TryGetValue(string key, out object? value) {
		throw new NotImplementedException();
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}
}
