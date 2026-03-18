using Tsinswreng.CsTreeTest;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessorMgr{
	public void RegisterGetProps(ITestNode Test){
		var register = Test.MkTestFnRegister(
			typeof(TestIPropAccessorMgr)
			,[typeof(IPropAccessorMgr)]
			,[nameof(IPropAccessorMgr.GetProps)]
			,nameof(TestIPropAccessorMgr) + "."
		);
		var R = register.Register;

		R("GetProps_Should_Return_Empty_When_TypeNotRegistered", async(O)=>{
			var Sut = NewSut();
			var Names = Sut.GetProps(typeof(object));
			if(Names.Count != 0){
				throw new Exception("GetProps should return empty list when target type is not registered");
			}
			return NIL;
		});
	}
}
