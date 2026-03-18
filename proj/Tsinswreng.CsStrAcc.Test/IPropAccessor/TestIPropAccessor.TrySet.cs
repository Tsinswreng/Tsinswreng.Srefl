using Tsinswreng.CsTreeTest;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessor{
	public ITestNode RegisterTrySet(ITestNode Node){
		var register = Node.MkTestFnRegister(
			typeof(TestIPropAccessor)
			,[typeof(IPropAccessor)]
			,[nameof(IPropAccessor.TrySet)]
			,nameof(TestIPropAccessor) + "."
		);
		var R = register.Register;

		R("TrySet_Should_ReturnTrue_AndMutate_When_KeyExists", async(o)=>{
			var sut = NewSut();
			var model = new DemoModel{ Age = 1, Name = "old" };
			var ok = sut.TrySet(model, nameof(DemoModel.Name), "new");
			if(!ok){
				throw new Exception("TrySet should return true for existing property");
			}
			if(model.Name != "new"){
				throw new Exception("TrySet should mutate target object");
			}
			return NIL;
		});

		R("TrySet_Should_ReturnFalse_When_KeyMissing", async(o)=>{
			var sut = NewSut();
			var model = new DemoModel{ Age = 1, Name = "old" };
			var ok = sut.TrySet(model, "NoSuchProp", "x");
			if(ok){
				throw new Exception("TrySet should return false for non-existing property");
			}
			return NIL;
		});

		return Node;
	}
}
