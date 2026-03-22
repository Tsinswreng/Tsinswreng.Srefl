using Tsinswreng.CsCore;

namespace Tsinswreng.Srefl;

public interface IInstMkr{
	public Type Target{get;set;}
	public obj MkInst();
	[Doc(@$"Create an empty List<T>
	and typeof(T) equals to {nameof(Target)}
	")]
	public obj MkList();
}
