using Tsinswreng.CsTreeTest;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessor{
	public void RegisterTrySet(ITestNode Test){
		var register = Test.MkTestFnRegister(
			typeof(TestIPropAccessor)
			,[typeof(IPropAccessor)]
			,[nameof(IPropAccessor.TrySet)]
			,nameof(TestIPropAccessor) + "."
		);
		var R = register.Register;

		R("TrySet_Should_ReturnTrue_AndMutate_When_KeyExists", async(O)=>{
			var Sut = NewSut();
			var Model = new DemoModel{ Age = 1, Name = "old" };
			var Ok = Sut.TrySet(Model, nameof(DemoModel.Name), "new");
			if(!Ok){
				throw new Exception("TrySet should return true for existing property");
			}
			if(Model.Name != "new"){
				throw new Exception("TrySet should mutate target object");
			}
			return NIL;
		});

		R("TrySet_Should_ReturnFalse_When_KeyMissing", async(O)=>{
			var Sut = NewSut();
			var Model = new DemoModel{ Age = 1, Name = "old" };
			var Ok = Sut.TrySet(Model, "NoSuchProp", "x");
			if(Ok){
				throw new Exception("TrySet should return false for non-existing property");
			}
			return NIL;
		});
	}
}
