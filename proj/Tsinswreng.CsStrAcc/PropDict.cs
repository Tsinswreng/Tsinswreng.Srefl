using Tsinswreng.CsCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;

namespace Tsinswreng.CsStrAcc;
[Doc(@$"
Property Dictionary of a object using {nameof(IPropAccessor)}

#H[{nameof(Keys)}][
	intersect of {nameof(PropAccessor.GetGetterNames)} and {nameof(PropAccessor.GetSetterNames)}
]
#H[{nameof(GetEnumerator)}][
	of {nameof(Keys)}
]
")]
public interface IPropDict:IDictionary<str, obj?>{
	public IPropAccessor PropAccessor{get;set;}
	public Type TargetType{get;set;}
	
	[Doc(@$"Should be assignable to {nameof(TargetType)}")]
	public obj? TargetObj{get;set;}
}

public class PropDict : IPropDict {
	protected IPropAccessor _PropAccessor = null!;
	protected Type _TargetType = null!;
	protected obj? _TargetObj;

	public PropDict(IPropAccessor PropAccessor, obj? TargetObj){
		this.PropAccessor = PropAccessor;
		this.TargetType = PropAccessor.TargetType;
		this.TargetObj = TargetObj;
	}

	public IPropAccessor PropAccessor{
		get => _PropAccessor;
		set{
			_PropAccessor = value ?? throw new ArgumentNullException(nameof(value));
			// PropAccessor 一旦切換，TargetType 要跟著對齊，避免狀態不一致。
			_TargetType = _PropAccessor.TargetType;
			EnsureTargetObjAssignable(_TargetObj);
		}
	}

	public Type TargetType{
		get => _TargetType;
		set{
			_TargetType = value ?? throw new ArgumentNullException(nameof(value));
			EnsureTargetObjAssignable(_TargetObj);
		}
	}

	public obj? TargetObj{
		get => _TargetObj;
		set{
			EnsureTargetObjAssignable(value);
			_TargetObj = value;
		}
	}

	public obj? this[str Key] {
		get{
			if(!PropAccessor.TryGet(TargetObj, Key, out var R)){
				throw new KeyNotFoundException($"key not readable: {Key}");
			}
			return R;
		}
		set{
			if(!PropAccessor.TrySet(TargetObj, Key, value)){
				throw new KeyNotFoundException($"key not writable: {Key}");
			}
		}
	}

	// Keys 定義爲可讀且可寫的鍵（Getter 與 Setter 的交集）。
	public ICollection<str> Keys => PropAccessor
		.GetGetterNames(TargetObj)
		.Intersect(PropAccessor.GetSetterNames(TargetObj))
		.ToImmutableSortedSet();

	public ICollection<obj?> Values => this.Select(Kv => Kv.Value).ToImmutableSortedSet();

	public i32 Count => Keys.Count;

	// 本字典支持索引器寫入，因此標記為可寫。
	public bool IsReadOnly => false;
	// 屬性字典是固定鍵集合，Add 在這裡按“設定屬性值”語義處理。
	public void Add(str Key, obj? Value){
		// 屬性字典是固定鍵集合，Add 在這裡按“設定屬性值”語義處理。
		if(!ContainsKey(Key)){
			throw new KeyNotFoundException($"key not found: {Key}");
		}
		this[Key] = Value;
	}

	public void Add(KeyValuePair<str, obj?> Item){
		Add(Item.Key, Item.Value);
	}

	public void Clear(){
		// 屬性集合由類型定義決定，不允許清空鍵集合。
		throw new NotSupportedException($"{nameof(PropDict)} does not support {nameof(Clear)}()");
	}

	public bool Contains(KeyValuePair<str, obj?> Item){
		if(!TryGetValue(Item.Key, out var V)){
			return false;
		}
		return Equals(V, Item.Value);
	}

	public bool ContainsKey(str Key){
		var GetterNames = PropAccessor.GetGetterNames(TargetObj);
		if(GetterNames.Contains(Key)){
			return true;
		}
		var SetterNames = PropAccessor.GetSetterNames(TargetObj);
		return SetterNames.Contains(Key);
	}

	public void CopyTo(KeyValuePair<str, obj?>[] Array, i32 ArrayIndex){
		if(Array == null){
			throw new ArgumentNullException(nameof(Array));
		}
		if(ArrayIndex < 0 || ArrayIndex > Array.Length){
			throw new ArgumentOutOfRangeException(nameof(ArrayIndex));
		}
		var Needed = Count;
		if(Array.Length - ArrayIndex < Needed){
			throw new ArgumentException("Target array is too small.");
		}
		var I = ArrayIndex;
		foreach(var Kv in this){
			Array[I++] = Kv;
		}
	}

	public IEnumerator<KeyValuePair<str, obj?>> GetEnumerator(){
		foreach(var Key in Keys){
			if(PropAccessor.TryGet(TargetObj, Key, out var R)){
				yield return new KeyValuePair<str, obj?>(Key, R);
			}
		}
	}

	public bool Remove(str Key){
		// 屬性集合是固定的，不能真正刪除鍵。
		throw new NotSupportedException($"{nameof(PropDict)} does not support {nameof(Remove)}({nameof(Key)})");
	}

	public bool Remove(KeyValuePair<str, obj?> Item){
		throw new NotSupportedException($"{nameof(PropDict)} does not support {nameof(Remove)}({nameof(Item)})");
	}

	public bool TryGetValue(str Key, out obj? Value){
		return PropAccessor.TryGet(TargetObj, Key, out Value);
	}

	IEnumerator IEnumerable.GetEnumerator(){
		return GetEnumerator();
	}

	protected void EnsureTargetObjAssignable(obj? Value){
		if(Value is null){
			return;
		}
		var GotType = Value.GetType();
		if(!TargetType.IsAssignableFrom(GotType)){
			throw new ArgumentException(
$@"{nameof(TargetObj)} must be assignable to {nameof(TargetType)}.
TargetType={TargetType}
Got={GotType}"
);
		}
	}
}
