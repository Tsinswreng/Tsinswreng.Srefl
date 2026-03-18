namespace Tsinswreng.CsStrAcc;
public interface IPropAccessorMgr{
	public IDictionary<Type, IPropAccessor> Type_PropAccessor{get;set;}
	#region 下面這些放到IPropAccessorMgr的擴展方法裏去
	public bool TryGet(obj? O, Type Target, str Key, out obj? R);

	public bool TrySet(obj? O, Type Target, str Key, obj? Value);
	
	public IReadOnlyList<str> GetProps(Type Target, OptGetGetterNames? Opt = null);
	
	public bool TryGetType(Type Target, str Key, out Type? Type);
	#endregion
	
}
