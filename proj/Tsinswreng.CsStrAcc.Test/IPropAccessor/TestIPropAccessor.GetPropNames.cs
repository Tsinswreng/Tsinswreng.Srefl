using Tsinswreng.CsTreeTest;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessor{
	public void RegisterGetPropNames(ITestNode Test){
		var register = Test.MkTestFnRegister(
			typeof(TestIPropAccessor)
			,[typeof(IPropAccessor)]
			,[nameof(IPropAccessor.GetPropNames)]
			,nameof(TestIPropAccessor) + "."
		);
		var R = register.Register;

		R("GetPropNames_Should_Contain_PublicProps", async(O)=>{
			var Sut = NewSut();
			var Model = new DemoModel();
			var Names = Sut.GetPropNames(Model);
			if(!Names.Contains(nameof(DemoModel.Age))){
				throw new Exception("GetPropNames missing Age");
			}
			if(!Names.Contains(nameof(DemoModel.Name))){
				throw new Exception("GetPropNames missing Name");
			}
			return NIL;
		});

		R("GetPropNames_Should_Return_SameInstance_On_RepeatedCalls", async(O)=>{
			var Sut = NewSut();
			var Model = new DemoModel();
			var A = Sut.GetPropNames(Model);
			var B = Sut.GetPropNames(Model);
			if(!ReferenceEquals(A, B)){
				throw new Exception("GetPropNames should return stable list instance across calls");
			}
			return NIL;
		});
	}
}
