using Tsinswreng.CsTreeTest;
using Tsinswreng.CsStrAcc;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessorMgr: ITester{
	private readonly IPropAccessorMgr _AccessorMgr;

	public TestIPropAccessorMgr(
		IPropAccessorMgr AccessorMgr
	){
		_AccessorMgr = AccessorMgr;
	}

	private IPropAccessorMgr NewSut(){
		return _AccessorMgr;
	}

	public ITestNode RegisterTestsInto(ITestNode? Node){
		Node ??= new TestNode();
		Node.Ordered = true;

		RegisterToPropDict(Node);

		return Node;
	}
}
