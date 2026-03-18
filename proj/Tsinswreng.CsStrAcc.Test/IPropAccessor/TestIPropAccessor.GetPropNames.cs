using Tsinswreng.CsTreeTest;
using Tsinswreng.CsStrAcc.Test.Models;

namespace Tsinswreng.CsStrAcc.Test;

public partial class TestIPropAccessor{
	public ITestNode RegisterGetGetterNames(ITestNode Node){
		var register = Node.MkTestFnRegister(
			typeof(TestIPropAccessor)
			,[typeof(IPropAccessor)]
			,[nameof(IPropAccessor.GetGetterNames)]
			,nameof(TestIPropAccessor) + "."
		);
		var R = register.Register;

		R("GetGetterNames_Should_Contain_PublicAndNonPublicGetters", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var model = new GeneralNsDerivedModel();
			var names = sut.GetGetterNames(model);
			if(!names.Contains(nameof(GeneralNsDerivedModel.Age))){
				throw new Exception("GetGetterNames missing Age");
			}
			if(!names.Contains(nameof(GeneralNsDerivedModel.Name))){
				throw new Exception("GetGetterNames missing Name");
			}
			if(!names.Contains(nameof(GeneralNsBaseModel.BaseId))){
				throw new Exception("GetGetterNames missing inherited BaseId");
			}
			return NIL;
		});

		R("GetGetterNames_Should_Exclude_Field_And_Method", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var model = new GeneralNsDerivedModel();
			var names = sut.GetGetterNames(model);
			if(names.Contains(nameof(GeneralNsDerivedModel.PublicField))){
				throw new Exception("GetGetterNames should not include fields");
			}
			if(names.Contains(nameof(GeneralNsDerivedModel.DerivedMethod))){
				throw new Exception("GetGetterNames should not include methods");
			}
			// GetGetterNames includes non-public getters, so PrivateProp should be visible
			if(!names.Contains("PrivateProp")){
				throw new Exception("GetGetterNames should include private property with getter");
			}
			if(!names.Contains("PrivateBaseProp")){
				throw new Exception("GetGetterNames should include inherited private property with getter");
			}
			return NIL;
		});

		R("GetGetterNames_Should_Work_For_TopLevelNamespaceModel", async(o)=>{
			var sut = NewSut(typeof(TopLevelNsModel));
			var model = new TopLevelNsModel();
			var names = sut.GetGetterNames(model);
			if(!names.Contains(nameof(TopLevelNsModel.TopId)) || !names.Contains(nameof(TopLevelNsModel.TopName))){
				throw new Exception("GetGetterNames should support top-level-namespace model");
			}
			return NIL;
		});

		R("GetGetterNames_Should_Work_For_NestedPublicModel", async(o)=>{
			var sut = NewSut(typeof(NestedTypeContainer.NestedPublicModel));
			var model = new NestedTypeContainer.NestedPublicModel();
			var names = sut.GetGetterNames(model);
			if(!names.Contains(nameof(NestedTypeContainer.NestedPublicModel.Level))){
				throw new Exception("GetGetterNames should include nested model property");
			}
			if(names.Contains(nameof(NestedTypeContainer.NestedPublicModel.PublicField))){
				throw new Exception("GetPropNames should not include nested model field");
			}
			return NIL;
		});

		R("GetGetterNames_Should_Return_SameInstance_On_RepeatedCalls", async(o)=>{
			var sut = NewSut(typeof(GeneralNsDerivedModel));
			var model = new GeneralNsDerivedModel();
			var a = sut.GetGetterNames(model);
			var b = sut.GetGetterNames(model);
			if(!ReferenceEquals(a, b)){
				throw new Exception("GetGetterNames should return stable list instance across calls");
			}
			return NIL;
		});

		return Node;
	}
}
