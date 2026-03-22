using Tsinswreng.CsTreeTest;
using Tsinswreng.Srefl;

namespace Tsinswreng.Srefl.Test;

public partial class TestIPropAccessor: ITester{
	private readonly IPropAccessorMgr _AccessorMgr;

	public TestIPropAccessor(
		IPropAccessorMgr AccessorMgr
	){
		_AccessorMgr = AccessorMgr;
	}

	/// IPropAccessor 是按單一 TargetType 工作的，
	/// 因此測試覆蓋多模型時需要按目標類型取 SUT。
	private IPropAccessor NewSut(Type TargetType){
		if(!_AccessorMgr.Type_PropAccessor.TryGetValue(TargetType, out var accessor)){
			throw new Exception($"IPropAccessorMgr does not contain accessor for target type: {TargetType}");
		}
		return accessor;
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
