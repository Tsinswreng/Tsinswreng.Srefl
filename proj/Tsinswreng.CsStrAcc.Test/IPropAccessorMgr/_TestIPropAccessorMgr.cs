using Tsinswreng.CsTreeTest;
using Tsinswreng.CsStrAcc;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessorMgr: ITester{
	private IPropAccessorMgr NewSut(){
		throw new NotImplementedException("TDD: IPropAccessorMgr implementation is not wired yet.");
	}

	public ITestNode RegisterTestsInto(ITestNode? Test){
		Test ??= new TestNode();
		Test.Ordered = true;

		RegisterCoreApis(Test);
		RegisterTryGet(Test);
		RegisterTrySet(Test);
		RegisterGetProps(Test);
		RegisterTryGetType(Test);
		return Test;
	}
}
