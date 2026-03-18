using Tsinswreng.CsTreeTest;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessorMgr{
	public void RegisterTrySet(ITestNode Test){
		var register = Test.MkTestFnRegister(
			typeof(TestIPropAccessorMgr)
			,[typeof(IPropAccessorMgr)]
			,[nameof(IPropAccessorMgr.TrySet)]
			,nameof(TestIPropAccessorMgr) + "."
		);
		var R = register.Register;

		R("TrySet_Should_ReturnFalse_When_TypeNotRegistered", async(O)=>{
			var Sut = NewSut();
			var Ok = Sut.TrySet(new object(), typeof(object), "Any", 1);
			if(Ok){
				throw new Exception("TrySet should return false when target type is not registered");
			}
			return NIL;
		});
	}
}
