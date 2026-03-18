using Tsinswreng.CsTreeTest;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessorMgr{
	public ITestNode RegisterTrySet(ITestNode Node){
		var register = Node.MkTestFnRegister(
			typeof(TestIPropAccessorMgr)
			,[typeof(IPropAccessorMgr)]
			,[nameof(IPropAccessorMgr.TrySet)]
			,nameof(TestIPropAccessorMgr) + "."
		);
		var R = register.Register;

		R("TrySet_Should_ReturnFalse_When_TypeNotRegistered", async(o)=>{
			var sut = NewSut();
			var ok = sut.TrySet(new object(), typeof(object), "Any", 1);
			if(ok){
				throw new Exception("TrySet should return false when target type is not registered");
			}
			return NIL;
		});
		return Node;
	}
}
