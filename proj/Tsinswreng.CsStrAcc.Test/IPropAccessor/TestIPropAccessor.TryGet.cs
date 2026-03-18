using Tsinswreng.CsTreeTest;
using Tsinswreng.CsStrAcc.Test.Models;

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
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var model = new GeneralNsDerivedModel{ Age = 19, Name = "n", BaseId = 11 };
			var ok = sut.TryGet(model, nameof(GeneralNsDerivedModel.Age), out var got);
			if(!ok){
				throw new Exception("TryGet should return true for existing property");
			}
			if(got is not int age || age != 19){
				throw new Exception("TryGet returned unexpected value for Age");
			}
			return NIL;
		});

		R("TryGet_Should_Read_InheritedPublicProp", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var model = new GeneralNsDerivedModel{ BaseId = 5566 };
			var ok = sut.TryGet(model, nameof(GeneralNsBaseModel.BaseId), out var got);
			if(!ok){
				throw new Exception("TryGet should support inherited public property");
			}
			if(got is not int value || value != 5566){
				throw new Exception("TryGet returned unexpected value for inherited BaseId");
			}
			return NIL;
		});

		R("TryGet_Should_ReturnFalse_For_Field", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var model = new GeneralNsDerivedModel{ PublicField = 99 };
			var ok = sut.TryGet(model, nameof(GeneralNsDerivedModel.PublicField), out _);
			if(ok){
				throw new Exception("TryGet should not read field members");
			}
			return NIL;
		});

		R("TryGet_Should_ReturnFalse_For_Method", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var model = new GeneralNsDerivedModel();
			var ok = sut.TryGet(model, nameof(GeneralNsDerivedModel.DerivedMethod), out _);
			if(ok){
				throw new Exception("TryGet should not resolve methods");
			}
			return NIL;
		});

		R("TryGet_Should_ReturnFalse_For_PrivateProp", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var model = new GeneralNsDerivedModel();
			var ok = sut.TryGet(model, "PrivateProp", out _);
			if(ok){
				throw new Exception("TryGet should not expose private property");
			}
			return NIL;
		});

		R("TryGet_Should_Work_For_TopLevelNamespaceModel", async(o)=>{
			var sut = NewSut(typeof(TopLevelNsModel));
			var model = new TopLevelNsModel{ TopId = 7 };
			var ok = sut.TryGet(model, nameof(TopLevelNsModel.TopId), out var got);
			if(!ok || got is not int value || value != 7){
				throw new Exception("TryGet should work for top-level-namespace model");
			}
			return NIL;
		});

		R("TryGet_Should_Work_For_NestedPublicModel", async(o)=>{
			var sut = NewSut(typeof(NestedTypeContainer.NestedPublicModel));
			var model = new NestedTypeContainer.NestedPublicModel{ Level = 3 };
			var ok = sut.TryGet(model, nameof(NestedTypeContainer.NestedPublicModel.Level), out var got);
			if(!ok || got is not int value || value != 3){
				throw new Exception("TryGet should work for nested public model property");
			}
			return NIL;
		});

		R("TryGet_Should_ReturnFalse_When_KeyMissing", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var model = new GeneralNsDerivedModel{ Age = 19, Name = "n" };
			var ok = sut.TryGet(model, "NoSuchProp", out _);
			if(ok){
				throw new Exception("TryGet should return false for non-existing property");
			}
			return NIL;
		});

		return Node;
	}
}
