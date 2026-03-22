using Tsinswreng.CsTreeTest;

namespace Tsinswreng.Srefl.Test;

public class StrAccTestMgr: DiEtTestMgr{
	public static StrAccTestMgr Inst = new();

	public override ITestNode RegisterTestsInto(ITestNode? Test){
		Test = this.TestNode;
		this.RegisterTester<TestIPropAccessor>();
		this.RegisterTester<TestIPropAccessorReg>();
		this.RegisterTester<TestIInstMkrReg>();
		return Test;
	}
}
