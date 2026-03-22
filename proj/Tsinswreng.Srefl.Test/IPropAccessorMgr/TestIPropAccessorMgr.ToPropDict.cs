using System.Collections.Generic;
using Tsinswreng.CsTreeTest;
using Tsinswreng.Srefl.Test.Models;

namespace Tsinswreng.Srefl.Test;

public partial class TestIPropAccessorMgr{
	public ITestNode RegisterToPropDict(ITestNode Node){
		var register = Node.MkTestFnRegister(
			typeof(TestIPropAccessorMgr)
			,[typeof(IPropAccessorMgr)]
			,[nameof(ExtnIPropAccessorMgr.ToPropDict)]
			,nameof(TestIPropAccessorMgr) + "."
		);
		var R = register.Register;

		R("ToPropDict_Should_Use_InputObject_AndTargetType", async(o)=>{
			var sut = NewSut();
			var model = new GeneralNsDerivedModel{ Age = 11, Name = "old" };

			var dict = sut.ToPropDict(model, typeof(GeneralNsDerivedModel));
			if(!ReferenceEquals(dict.TargetObj, model)){
				throw new Exception("ToPropDict(obj, Type) should keep the same target object instance");
			}
			if(dict.TargetType != typeof(GeneralNsDerivedModel)){
				throw new Exception("ToPropDict(obj, Type) returned wrong TargetType");
			}
			if(!dict.TryGetValue(nameof(GeneralNsDerivedModel.Age), out var got) || got is not int age || age != 11){
				throw new Exception("ToPropDict(obj, Type) should support reading properties from input object");
			}
			return NIL;
		});

		R("ToPropDict_Should_Allow_SetValue_Through_Dict", async(o)=>{
			var sut = NewSut();
			var model = new GeneralNsDerivedModel{ Name = "old" };

			var dict = sut.ToPropDict(model, typeof(GeneralNsDerivedModel));
			dict[nameof(GeneralNsDerivedModel.Name)] = "new";
			if(model.Name != "new"){
				throw new Exception("ToPropDict(obj, Type) should write back to original object");
			}
			return NIL;
		});

		R("ToPropDict_Generic_Should_Use_TypeofT_AndInputObject", async(o)=>{
			var sut = NewSut();
			var model = new GeneralNsDerivedModel{ Age = 21 };

			var dict = sut.ToPropDict(model);
			if(dict.TargetType != typeof(GeneralNsDerivedModel)){
				throw new Exception("ToPropDict<T>(T) should use typeof(T) as TargetType");
			}
			if(!ReferenceEquals(dict.TargetObj, model)){
				throw new Exception("ToPropDict<T>(T) should keep the same target object instance");
			}
			if(!dict.TryGetValue(nameof(GeneralNsDerivedModel.Age), out var got) || got is not int age || age != 21){
				throw new Exception("ToPropDict<T>(T) should support reading properties from input object");
			}
			return NIL;
		});

		R("ToPropDict_Should_Throw_When_TargetType_NotRegistered", async(o)=>{
			var sut = NewSut();
			var model = new GeneralNsDerivedModel();
			var threw = false;
			try{
				_ = sut.ToPropDict(model, typeof(List<int>));
			}catch(KeyNotFoundException){
				threw = true;
			}
			if(!threw){
				throw new Exception("ToPropDict(obj, Type) should throw KeyNotFoundException for unregistered target type");
			}
			return NIL;
		});

		return Node;
	}
}
