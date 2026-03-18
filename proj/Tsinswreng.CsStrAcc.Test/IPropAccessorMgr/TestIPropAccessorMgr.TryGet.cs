using Tsinswreng.CsTreeTest;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessorMgr{
	public ITestNode RegisterTryGet(ITestNode Node){
		var register = Node.MkTestFnRegister(
			typeof(TestIPropAccessorMgr)
			,[typeof(IPropAccessorMgr)]
			,[nameof(IPropAccessorMgr.TryGet)]
			,nameof(TestIPropAccessorMgr) + "."
		);
		var R = register.Register;

		R("TryGet_Should_ReturnFalse_When_TypeNotRegistered", async(o)=>{
			var sut = NewSut();
			var ok = sut.TryGet(new object(), typeof(object), "Any", out _);
			if(ok){
				throw new Exception("TryGet should return false when target type is not registered");
			}
			return NIL;
		});
		return Node;
	}
}
