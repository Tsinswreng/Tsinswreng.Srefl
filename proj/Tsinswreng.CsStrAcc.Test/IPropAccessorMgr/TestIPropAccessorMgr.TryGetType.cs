using Tsinswreng.CsTreeTest;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessorMgr{
	public void RegisterTryGetType(ITestNode Test){
		var register = Test.MkTestFnRegister(
			typeof(TestIPropAccessorMgr)
			,[typeof(IPropAccessorMgr)]
			,[nameof(IPropAccessorMgr.TryGetType)]
			,nameof(TestIPropAccessorMgr) + "."
		);
		var R = register.Register;

		R("TryGetType_Should_ReturnFalse_When_TypeNotRegistered", async(O)=>{
			var Sut = NewSut();
			var Ok = Sut.TryGetType(typeof(object), "Any", out _);
			if(Ok){
				throw new Exception("TryGetType should return false when target type is not registered");
			}
			return NIL;
		});
	}
}
