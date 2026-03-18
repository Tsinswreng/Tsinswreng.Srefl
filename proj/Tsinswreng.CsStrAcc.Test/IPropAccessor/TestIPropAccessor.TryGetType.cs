using Tsinswreng.CsTreeTest;
using Tsinswreng.CsStrAcc.Test.Models;

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
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var ok = sut.TryGetType(nameof(GeneralNsDerivedModel.Name), out var t);
			if(!ok){
				throw new Exception("TryGetType should return true for existing property");
			}
			if(t != typeof(string)){
				throw new Exception($"TryGetType expected {typeof(string)}, got {t}");
			}
			return NIL;
		});

		R("TryGetType_Should_Work_For_InheritedPublicProp", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var ok = sut.TryGetType(nameof(GeneralNsBaseModel.BaseId), out var t);
			if(!ok || t != typeof(int)){
				throw new Exception("TryGetType should return inherited property declared type");
			}
			return NIL;
		});

		R("TryGetType_Should_ReturnFalse_For_Field", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var ok = sut.TryGetType(nameof(GeneralNsDerivedModel.PublicField), out _);
			if(ok){
				throw new Exception("TryGetType should not resolve fields");
			}
			return NIL;
		});

		R("TryGetType_Should_ReturnFalse_For_Method", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var ok = sut.TryGetType(nameof(GeneralNsDerivedModel.DerivedMethod), out _);
			if(ok){
				throw new Exception("TryGetType should not resolve methods");
			}
			return NIL;
		});

		R("TryGetType_Should_ReturnFalse_For_PrivateProp", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var ok = sut.TryGetType("PrivateProp", out _);
			if(ok){
				throw new Exception("TryGetType should not expose private property");
			}
			return NIL;
		});

		R("TryGetType_Should_Work_For_TopLevelNamespaceModel", async(o)=>{
			var sut = NewSut(typeof(TopLevelNsModel));
			var ok = sut.TryGetType(nameof(TopLevelNsModel.TopName), out var t);
			if(!ok || t != typeof(string)){
				throw new Exception("TryGetType should support top-level-namespace model");
			}
			return NIL;
		});

		R("TryGetType_Should_Work_For_NestedPublicModel", async(o)=>{
			var sut = NewSut(typeof(NestedTypeContainer.NestedPublicModel));
			var ok = sut.TryGetType(nameof(NestedTypeContainer.NestedPublicModel.Level), out var t);
			if(!ok || t != typeof(int)){
				throw new Exception("TryGetType should support nested public model property");
			}
			return NIL;
		});

		R("TryGetType_Should_ReturnFalse_When_KeyMissing", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var ok = sut.TryGetType("NoSuchProp", out _);
			if(ok){
				throw new Exception("TryGetType should return false for non-existing property");
			}
			return NIL;
		});

		return Node;
	}
}
