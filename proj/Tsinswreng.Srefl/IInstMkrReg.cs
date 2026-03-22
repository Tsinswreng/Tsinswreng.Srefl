using Tsinswreng.CsCore;

namespace Tsinswreng.Srefl;

public interface IInstMkrReg{
	public IDictionary<Type, IInstMkr> Type_InstMkr{get;set;}
}
