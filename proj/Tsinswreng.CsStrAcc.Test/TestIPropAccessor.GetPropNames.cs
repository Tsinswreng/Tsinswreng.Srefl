using Tsinswreng.CsTreeTest;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessor{
	public ITestNode RegisterGetPropNames(ITestNode Node){
		var register = Node.MkTestFnRegister(
			typeof(TestIPropAccessor)
			,[typeof(IPropAccessor)]
			,[nameof(IPropAccessor.GetPropNames)]
			,nameof(TestIPropAccessor) + "."
		);
		var R = register.Register;

		R("GetPropNames_Should_Contain_PublicProps", async(o)=>{
			var sut = NewSut();
			var model = new DemoModel();
			var names = sut.GetPropNames(model);
			if(!names.Contains(nameof(DemoModel.Age))){
				throw new Exception("GetPropNames missing Age");
			}
			if(!names.Contains(nameof(DemoModel.Name))){
				throw new Exception("GetPropNames missing Name");
			}
			return NIL;
		});

		R("GetPropNames_Should_Return_SameInstance_On_RepeatedCalls", async(o)=>{
			var sut = NewSut();
			var model = new DemoModel();
			var a = sut.GetPropNames(model);
			var b = sut.GetPropNames(model);
			if(!ReferenceEquals(a, b)){
				throw new Exception("GetPropNames should return stable list instance across calls");
			}
			return NIL;
		});

		return Node;
	}
}
