using Tsinswreng.CsTreeTest;
using Tsinswreng.CsStrAcc.Test.Models;

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
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var model = new GeneralNsDerivedModel{ Age = 1, Name = "old" };
			var ok = sut.TrySet(model, nameof(GeneralNsDerivedModel.Name), "new");
			if(!ok){
				throw new Exception("TrySet should return true for existing property");
			}
			if(model.Name != "new"){
				throw new Exception("TrySet should mutate target object");
			}
			return NIL;
		});

		R("TrySet_Should_Set_InheritedPublicProp", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var model = new GeneralNsDerivedModel{ BaseId = 3 };
			var ok = sut.TrySet(model, nameof(GeneralNsBaseModel.BaseId), 10);
			if(!ok){
				throw new Exception("TrySet should support inherited public property");
			}
			if(model.BaseId != 10){
				throw new Exception("TrySet should mutate inherited BaseId");
			}
			return NIL;
		});

		R("TrySet_Should_ReturnFalse_For_ReadOnlyProp", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var model = new GeneralNsDerivedModel();
			var ok = sut.TrySet(model, nameof(GeneralNsBaseModel.ReadOnlyBaseProp), 123);
			if(ok){
				throw new Exception("TrySet should fail on read-only property");
			}
			return NIL;
		});

		R("TrySet_Should_ReturnFalse_For_Field", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var model = new GeneralNsDerivedModel();
			var ok = sut.TrySet(model, nameof(GeneralNsDerivedModel.PublicField), 123);
			if(ok){
				throw new Exception("TrySet should not write field members");
			}
			return NIL;
		});

		R("TrySet_Should_ReturnFalse_For_Method", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var model = new GeneralNsDerivedModel();
			var ok = sut.TrySet(model, nameof(GeneralNsDerivedModel.DerivedMethod), "x");
			if(ok){
				throw new Exception("TrySet should not target methods");
			}
			return NIL;
		});

		R("TrySet_Should_ReturnFalse_For_PrivateProp", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var model = new GeneralNsDerivedModel();
			var ok = sut.TrySet(model, "PrivateProp", "x");
			if(ok){
				throw new Exception("TrySet should not expose private property");
			}
			return NIL;
		});

		R("TrySet_Should_ReturnFalse_When_KeyMissing", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var model = new GeneralNsDerivedModel{ Age = 1, Name = "old" };
			var ok = sut.TrySet(model, "NoSuchProp", "x");
			if(ok){
				throw new Exception("TrySet should return false for non-existing property");
			}
			return NIL;
		});

		return Node;
	}
}
