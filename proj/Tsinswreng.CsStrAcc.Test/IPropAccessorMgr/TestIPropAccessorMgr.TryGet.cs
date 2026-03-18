using Tsinswreng.CsTreeTest;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessorMgr{
	public void RegisterTryGet(ITestNode Test){
		var register = Test.MkTestFnRegister(
			typeof(TestIPropAccessorMgr)
			,[typeof(IPropAccessorMgr)]
			,[nameof(IPropAccessorMgr.TryGet)]
			,nameof(TestIPropAccessorMgr) + "."
		);
		var R = register.Register;

		R("TryGet_Should_ReturnFalse_When_TypeNotRegistered", async(O)=>{
			var Sut = NewSut();
			var Ok = Sut.TryGet(new object(), typeof(object), "Any", out _);
			if(Ok){
				throw new Exception("TryGet should return false when target type is not registered");
			}
			return NIL;
		});
	}
}
