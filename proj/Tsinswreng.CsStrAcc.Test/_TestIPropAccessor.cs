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

	public ITestNode RegisterTestsInto(ITestNode? Node){
		Node ??= new TestNode();
		Node.Ordered = true;

		RegisterTryGet(Node);
		RegisterTrySet(Node);
		RegisterGetPropNames(Node);
		RegisterTryGetType(Node);
		return Node;
	}
}
