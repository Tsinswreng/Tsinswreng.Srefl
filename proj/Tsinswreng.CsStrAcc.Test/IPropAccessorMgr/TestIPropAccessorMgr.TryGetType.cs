using Tsinswreng.CsTreeTest;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessorMgr{
	public ITestNode RegisterTryGetType(ITestNode Node){
		var register = Node.MkTestFnRegister(
			typeof(TestIPropAccessorMgr)
			,[typeof(IPropAccessorMgr)]
			,[nameof(IPropAccessorMgr.TryGetType)]
			,nameof(TestIPropAccessorMgr) + "."
		);
		var R = register.Register;

		R("TryGetType_Should_ReturnFalse_When_TypeNotRegistered", async(o)=>{
			var sut = NewSut();
			var ok = sut.TryGetType(typeof(object), "Any", out _);
			if(ok){
				throw new Exception("TryGetType should return false when target type is not registered");
			}
			return NIL;
		});
		return Node;
	}
}
