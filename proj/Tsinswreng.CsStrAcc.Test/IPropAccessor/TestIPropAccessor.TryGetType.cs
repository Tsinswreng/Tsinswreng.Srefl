using Tsinswreng.CsTreeTest;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessor{
	public void RegisterTryGetType(ITestNode Test){
		var register = Test.MkTestFnRegister(
			typeof(TestIPropAccessor)
			,[typeof(IPropAccessor)]
			,[nameof(IPropAccessor.TryGetType)]
			,nameof(TestIPropAccessor) + "."
		);
		var R = register.Register;

		R("TryGetType_Should_ReturnDeclaredType", async(O)=>{
			var Sut = NewSut();
			var Ok = Sut.TryGetType(nameof(DemoModel.Name), out var Type);
			if(!Ok){
				throw new Exception("TryGetType should return true for existing property");
			}
			if(Type != typeof(string)){
				throw new Exception($"TryGetType expected {typeof(string)}, got {Type}");
			}
			return NIL;
		});

		R("TryGetType_Should_ReturnFalse_When_KeyMissing", async(O)=>{
			var Sut = NewSut();
			var Ok = Sut.TryGetType("NoSuchProp", out _);
			if(Ok){
				throw new Exception("TryGetType should return false for non-existing property");
			}
			return NIL;
		});
	}
}
