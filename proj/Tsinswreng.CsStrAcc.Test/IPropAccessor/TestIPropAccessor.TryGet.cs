using Tsinswreng.CsTreeTest;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessor{
	public ITestNode RegisterTryGet(ITestNode Node){
		var register = Node.MkTestFnRegister(
			typeof(TestIPropAccessor)
			,[typeof(IPropAccessor)]
			,[nameof(IPropAccessor.TryGet)]
			,nameof(TestIPropAccessor) + "."
		);
		var R = register.Register;

		R("TryGet_Should_ReturnTrue_AndValue_When_KeyExists", async(o)=>{
			var sut = NewSut();
			var model = new DemoModel{ Age = 19, Name = "n" };
			var ok = sut.TryGet(model, nameof(DemoModel.Age), out var got);
			if(!ok){
				throw new Exception("TryGet should return true for existing property");
			}
			if(got is not int age || age != 19){
				throw new Exception("TryGet returned unexpected value for Age");
			}
			return NIL;
		});

		R("TryGet_Should_ReturnFalse_When_KeyMissing", async(o)=>{
			var sut = NewSut();
			var model = new DemoModel{ Age = 19, Name = "n" };
			var ok = sut.TryGet(model, "NoSuchProp", out _);
			if(ok){
				throw new Exception("TryGet should return false for non-existing property");
			}
			return NIL;
		});

		return Node;
	}
}
