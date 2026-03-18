using Tsinswreng.CsTreeTest;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessor{
	public void RegisterTryGet(ITestNode Test){
		var register = Test.MkTestFnRegister(
			typeof(TestIPropAccessor)
			,[typeof(IPropAccessor)]
			,[nameof(IPropAccessor.TryGet)]
			,nameof(TestIPropAccessor) + "."
		);
		var R = register.Register;

		R("TryGet_Should_ReturnTrue_AndValue_When_KeyExists", async(O)=>{
			var Sut = NewSut();
			var Model = new DemoModel{ Age = 19, Name = "n" };
			var Ok = Sut.TryGet(Model, nameof(DemoModel.Age), out var Got);
			if(!Ok){
				throw new Exception("TryGet should return true for existing property");
			}
			if(Got is not int Age || Age != 19){
				throw new Exception("TryGet returned unexpected value for Age");
			}
			return NIL;
		});

		R("TryGet_Should_ReturnFalse_When_KeyMissing", async(O)=>{
			var Sut = NewSut();
			var Model = new DemoModel{ Age = 19, Name = "n" };
			var Ok = Sut.TryGet(Model, "NoSuchProp", out _);
			if(Ok){
				throw new Exception("TryGet should return false for non-existing property");
			}
			return NIL;
		});
	}
}
