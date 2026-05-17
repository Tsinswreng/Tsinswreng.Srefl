using Tsinswreng.CsTreeTest;

namespace Tsinswreng.Srefl.Test;

public class StrAccTestMgr: DiEtTestMgr{
	public static StrAccTestMgr Inst = new();

	public override ITestNode RegisterTestsInto(ITestNode? Test){
		Test = this.TestNode;
		this.RegisterTester<TestIPropAccessor>();
		this.RegisterTester<TestIPropAccessorReg>();
		// 當前 SrcGen 只生成 PropAccessorReg，尚未生成 InstMkrReg，
		// 因此先不把 TestIInstMkrReg 掛進測試樹。
		return Test;
	}
}
