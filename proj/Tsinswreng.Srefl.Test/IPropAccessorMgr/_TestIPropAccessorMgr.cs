using Tsinswreng.CsTreeTest;
using Tsinswreng.Srefl;

namespace Tsinswreng.Srefl.Test;

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
