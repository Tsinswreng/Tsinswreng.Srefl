using Tsinswreng.CsTreeTest;
using Tsinswreng.Srefl;

namespace Tsinswreng.Srefl.Test;

public partial class TestIPropAccessorReg: ITester{
	private readonly IPropAccessorReg _AccessorMgr;

	public TestIPropAccessorReg(
		IPropAccessorReg AccessorMgr
	){
		_AccessorMgr = AccessorMgr;
	}

	private IPropAccessorReg NewSut(){
		return _AccessorMgr;
	}

	public ITestNode RegisterTestsInto(ITestNode? Node){
		Node ??= new TestNode();
		Node.Ordered = true;

		RegisterToPropDict(Node);

		return Node;
	}
}
