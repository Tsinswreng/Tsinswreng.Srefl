using Tsinswreng.CsTreeTest;
using Tsinswreng.CsStrAcc;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessor: ITester{
	private sealed class DemoModel{
		public int Age { get; set; }
		public string? Name { get; set; }
	}

	private IPropAccessor NewSut(){
		throw new NotImplementedException("TDD: IPropAccessor implementation is not wired yet.");
	}

	public ITestNode RegisterTestsInto(ITestNode? Test){
		Test ??= new TestNode();
		Test.Ordered = true;

		RegisterTryGet(Test);
		RegisterTrySet(Test);
		RegisterGetPropNames(Test);
		RegisterTryGetType(Test);
		return Test;
	}
}
