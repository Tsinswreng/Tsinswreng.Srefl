using Tsinswreng.CsTreeTest;
using Tsinswreng.CsStrAcc;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessor: ITester{
	/// IPropAccessor 是按單一 TargetType 工作的，
	/// 因此測試覆蓋多模型時需要按目標類型取 SUT。
	private IPropAccessor NewSut(Type TargetType){
		throw new NotImplementedException($"TDD: IPropAccessor implementation is not wired yet. TargetType={TargetType}");
	}

	private IPropAccessor NewSut(){
		return NewSut(typeof(Models.GeneralNsDerivedModel));
	}

	public ITestNode RegisterTestsInto(ITestNode? Node){
		Node ??= new TestNode();
		Node.Ordered = true;

		RegisterTryGet(Node);
		RegisterTrySet(Node);
		RegisterGetGetterNames(Node);
		RegisterGetSetterNames(Node);
		RegisterTryGetType(Node);
		return Node;
	}
}
