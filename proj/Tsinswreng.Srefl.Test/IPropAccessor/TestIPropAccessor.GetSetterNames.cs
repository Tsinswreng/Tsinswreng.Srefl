using Tsinswreng.CsTreeTest;
using Tsinswreng.CsStrAcc.Test.Models;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessor{
	public ITestNode RegisterGetSetterNames(ITestNode Node){
		var register = Node.MkTestFnRegister(
			typeof(TestIPropAccessor)
			,[typeof(IPropAccessor)]
			,[nameof(IPropAccessor.GetSetterNames)]
			,nameof(TestIPropAccessor) + "."
		);
		var R = register.Register;

		R("GetSetterNames_Should_Contain_PublicAndNonPublicSetters", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var model = new GeneralNsDerivedModel();
			var names = sut.GetSetterNames(model);
			if(!names.Contains(nameof(GeneralNsDerivedModel.Age))){
				throw new Exception("GetSetterNames missing Age");
			}
			if(!names.Contains(nameof(GeneralNsDerivedModel.Name))){
				throw new Exception("GetSetterNames missing Name");
			}
			if(!names.Contains(nameof(GeneralNsBaseModel.BaseId))){
				throw new Exception("GetSetterNames missing inherited BaseId");
			}
			return NIL;
		});

		R("GetSetterNames_Should_Exclude_Field_Method_And_ReadOnlyProp", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var model = new GeneralNsDerivedModel();
			var names = sut.GetSetterNames(model);
			if(names.Contains(nameof(GeneralNsDerivedModel.PublicField))){
				throw new Exception("GetSetterNames should not include fields");
			}
			if(names.Contains(nameof(GeneralNsDerivedModel.DerivedMethod))){
				throw new Exception("GetSetterNames should not include methods");
			}
			if(names.Contains(nameof(GeneralNsBaseModel.ReadOnlyBaseProp))){
				throw new Exception("GetSetterNames should not include read-only property");
			}
			// GetSetterNames includes non-public setters, so PrivateProp should be visible
			if(!names.Contains("PrivateProp")){
				throw new Exception("GetSetterNames should include private property with setter");
			}
			return NIL;
		});

		R("GetSetterNames_Should_Work_For_TopLevelNamespaceModel", async(o)=>{
			var sut = NewSut(typeof(TopLevelNsModel));
			var model = new TopLevelNsModel();
			var names = sut.GetSetterNames(model);
			if(!names.Contains(nameof(TopLevelNsModel.TopId)) || !names.Contains(nameof(TopLevelNsModel.TopName))){
				throw new Exception("GetSetterNames should support top-level-namespace model");
			}
			return NIL;
		});

		R("GetSetterNames_Should_Work_For_NestedPublicModel", async(o)=>{
			var sut = NewSut(typeof(NestedTypeContainer.NestedPublicModel));
			var model = new NestedTypeContainer.NestedPublicModel();
			var names = sut.GetSetterNames(model);
			if(!names.Contains(nameof(NestedTypeContainer.NestedPublicModel.Level))){
				throw new Exception("GetSetterNames should include nested model property");
			}
			if(names.Contains(nameof(NestedTypeContainer.NestedPublicModel.PublicField))){
				throw new Exception("GetSetterNames should not include nested model field");
			}
			return NIL;
		});

		R("GetSetterNames_Should_Return_SameInstance_On_RepeatedCalls", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var model = new GeneralNsDerivedModel();
			var a = sut.GetSetterNames(model);
			var b = sut.GetSetterNames(model);
			if(!ReferenceEquals(a, b)){
				throw new Exception("GetSetterNames should return stable list instance across calls");
			}
			return NIL;
		});

		return Node;
	}
}
