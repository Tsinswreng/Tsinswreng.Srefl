using Tsinswreng.CsTreeTest;
using Tsinswreng.Srefl.Test.Models;

namespace Tsinswreng.Srefl.Test;

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

		R("TrySet_Should_Write_PrivateProp_When_NonPublicSetterIsSupported", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var model = new GeneralNsDerivedModel();
			var ok = sut.TrySet(model, "PrivateProp", "x");
			if(!ok){
				throw new Exception("TrySet should support private property setter");
			}
			if(!sut.TryGet(model, "PrivateProp", out var got) || got as string != "x"){
				throw new Exception("TrySet did not update private property as expected");
			}
			return NIL;
		});

		R("TrySet_Should_ReturnFalse_When_ObjectIsNull", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var ok = sut.TrySet(null, nameof(GeneralNsDerivedModel.Age), 11);
			if(ok){
				throw new Exception("TrySet should return false when object is null");
			}
			return NIL;
		});

		R("TrySet_Should_ReturnFalse_When_ObjectTypeMismatches_TargetType", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var wrongObj = new TopLevelNsModel{ TopId = 1 };
			var ok = sut.TrySet(wrongObj, nameof(GeneralNsDerivedModel.Age), 22);
			if(ok){
				throw new Exception("TrySet should return false when object type mismatches target type");
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
