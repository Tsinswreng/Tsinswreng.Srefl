using Tsinswreng.CsTreeTest;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessorMgr{
	public ITestNode RegisterTryGet(ITestNode Node){
		var register = Node.MkTestFnRegister(
			typeof(TestIPropAccessorMgr)
			,[typeof(IPropAccessorMgr)]
			,[nameof(IPropAccessorMgr.TryGet)]
			,nameof(TestIPropAccessorMgr) + "."
		);
		var R = register.Register;

		R("TryGet_Should_ReturnFalse_When_TypeNotRegistered", async(o)=>{
			var sut = NewSut();
			var ok = sut.TryGet(new object(), typeof(object), "Any", out _);
			if(ok){
				throw new Exception("TryGet should return false when target type is not registered");
			}
			return NIL;
		});
		return Node;
	}

	public ITestNode RegisterTrySet(ITestNode Node){
		var register = Node.MkTestFnRegister(
			typeof(TestIPropAccessorMgr)
			,[typeof(IPropAccessorMgr)]
			,[nameof(IPropAccessorMgr.TrySet)]
			,nameof(TestIPropAccessorMgr) + "."
		);
		var R = register.Register;

		R("TrySet_Should_ReturnFalse_When_TypeNotRegistered", async(o)=>{
			var sut = NewSut();
			var ok = sut.TrySet(new object(), typeof(object), "Any", 1);
			if(ok){
				throw new Exception("TrySet should return false when target type is not registered");
			}
			return NIL;
		});
		return Node;
	}

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

	public ITestNode RegisterTryGetType(ITestNode Node){
		var register = Node.MkTestFnRegister(
			typeof(TestIPropAccessorMgr)
			,[typeof(IPropAccessorMgr)]
			,[nameof(IPropAccessorMgr.TryGetType)]
			,nameof(TestIPropAccessorMgr) + "."
		);
		var R = register.Register;

		R("TryGetType_Should_ReturnFalse_When_TypeNotRegistered", async(o)=>{
			var sut = NewSut();
			var ok = sut.TryGetType(typeof(object), "Any", out _);
			if(ok){
				throw new Exception("TryGetType should return false when target type is not registered");
			}
			return NIL;
		});
		return Node;
	}
}
