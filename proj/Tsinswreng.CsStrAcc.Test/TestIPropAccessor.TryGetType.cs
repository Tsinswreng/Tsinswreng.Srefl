using Tsinswreng.CsTreeTest;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessor{
	public ITestNode RegisterTryGetType(ITestNode Node){
		var register = Node.MkTestFnRegister(
			typeof(TestIPropAccessor)
			,[typeof(IPropAccessor)]
			,[nameof(IPropAccessor.TryGetType)]
			,nameof(TestIPropAccessor) + "."
		);
		var R = register.Register;

		R("TryGetType_Should_ReturnDeclaredType", async(o)=>{
			var sut = NewSut();
			var ok = sut.TryGetType(nameof(DemoModel.Name), out var t);
			if(!ok){
				throw new Exception("TryGetType should return true for existing property");
			}
			if(t != typeof(string)){
				throw new Exception($"TryGetType expected {typeof(string)}, got {t}");
			}
			return NIL;
		});

		R("TryGetType_Should_ReturnFalse_When_KeyMissing", async(o)=>{
			var sut = NewSut();
			var ok = sut.TryGetType("NoSuchProp", out _);
			if(ok){
				throw new Exception("TryGetType should return false for non-existing property");
			}
			return NIL;
		});

		return Node;
	}
}
