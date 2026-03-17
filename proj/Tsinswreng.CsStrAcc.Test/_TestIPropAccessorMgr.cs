using Tsinswreng.CsTreeTest;
using Tsinswreng.CsStrAcc;
namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessorMgr: ITester{
	private IPropAccessorMgr NewSut(){
		throw new NotImplementedException("TDD: IPropAccessorMgr implementation is not wired yet.");
	}

	public ITestNode RegisterTestsInto(ITestNode? Node){
		Node ??= new TestNode();
		Node.Ordered = true;

		RegisterTryGet(Node);
		RegisterTrySet(Node);
		RegisterGetProps(Node);
		RegisterTryGetType(Node);
		return Node;
	}
}
