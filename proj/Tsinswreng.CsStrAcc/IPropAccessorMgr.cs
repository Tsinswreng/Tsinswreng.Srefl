namespace Tsinswreng.CsStrAcc;
public interface IPropAccessorMgr{
	public IDictionary<Type, IPropAccessor> Type_PropAccessor{get;set;}
}
