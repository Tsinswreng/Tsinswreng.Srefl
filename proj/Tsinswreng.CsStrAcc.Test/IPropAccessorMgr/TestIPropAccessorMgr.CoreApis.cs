using Tsinswreng.CsTreeTest;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessorMgr{
	public void RegisterCoreApis(ITestNode Test){
		var register = Test.MkTestFnRegister(
			typeof(TestIPropAccessorMgr)
			,[typeof(IPropAccessorMgr)]
			,[nameof(IPropAccessorMgr.Type_PropAccessor)]
			,nameof(TestIPropAccessorMgr) + "."
		);
		var R = register.Register;

		R("Type_PropAccessor_Should_BeReadable", async(O)=>{
			var Sut = NewSut();
			_ = Sut.Type_PropAccessor;
			return NIL;
		});
	}
}
