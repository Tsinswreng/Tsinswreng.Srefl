using Tsinswreng.CsTreeTest;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessorMgr{
	public ITestNode RegisterGetProps(ITestNode Node){
		var register = Node.MkTestFnRegister(
			typeof(TestIPropAccessorMgr)
			,[typeof(IPropAccessorMgr)]
			,[nameof(IPropAccessorMgr.GetProps)]
			,nameof(TestIPropAccessorMgr) + "."
		);
		var R = register.Register;

		R("GetProps_Should_Return_Empty_When_TypeNotRegistered", async(o)=>{
			var sut = NewSut();
			var names = sut.GetProps(typeof(object));
			if(names.Count != 0){
				throw new Exception("GetProps should return empty list when target type is not registered");
			}
			return NIL;
		});
		return Node;
	}
}
