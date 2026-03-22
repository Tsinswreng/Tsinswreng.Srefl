using Tsinswreng.CsTreeTest;
using Tsinswreng.Srefl;

namespace Tsinswreng.Srefl.Test;

public partial class TestIInstMkrReg: ITester{
	private readonly IInstMkrReg _InstMkrReg;

	public TestIInstMkrReg(
		IInstMkrReg InstMkrReg
	){
		_InstMkrReg = InstMkrReg;
	}

	private IInstMkrReg NewSut(){
		return _InstMkrReg;
	}

	public ITestNode RegisterTestsInto(ITestNode? Node){
		Node ??= new TestNode();
		Node.Ordered = true;

		RegisterInstMkrMethods(Node);

		return Node;
	}
}
