using System.Collections.Generic;
using Tsinswreng.CsTreeTest;
using Tsinswreng.Srefl;
using Tsinswreng.Srefl.Test.Models;

namespace Tsinswreng.Srefl.Test;

public partial class TestIInstMkrReg{
	private void RegisterInstMkrMethods(ITestNode Node){
		var register = Node.MkTestFnRegister(
			typeof(TestIInstMkrReg)
			,[typeof(IInstMkrReg)]
			,[nameof(RegisterInstMkrMethods)]
			,nameof(TestIInstMkrReg) + "."
		);
		var R = register.Register;

		R("Type_InstMkr_Contains_RegisteredTypes", async(o)=>{
			var sut = NewSut();
			var dict = sut.Type_InstMkr;

			// 检查所有注册的类型都在字典中
			var expectedTypes = new[]{
				typeof(TopLevelNsModel),
				typeof(GeneralNsBaseModel),
				typeof(GeneralNsDerivedModel),
				typeof(NestedTypeContainer.NestedPublicModel),
				typeof(NestedTypeContainer.NestedInternalModel),
			};

			foreach(var type in expectedTypes){
				if(!dict.ContainsKey(type)){
					throw new Exception($"Type {type.Name} not found in Type_InstMkr dictionary");
				}
			}

			return NIL;
		});

		R("MkInst_Creates_TopLevelNsModel", async(o)=>{
			var sut = NewSut();
			var dict = sut.Type_InstMkr;

			// 测试创建 TopLevelNsModel 实例
			if(!dict.TryGetValue(typeof(TopLevelNsModel), out var mkr)){
				throw new Exception($"No InstMkr for {nameof(TopLevelNsModel)}");
			}

			var inst = mkr.MkInst();
			if(inst is not TopLevelNsModel model){
				throw new Exception($"MkInst returned {inst?.GetType().Name ?? "null"}, expected TopLevelNsModel");
			}

			// 验证创建的实例能够设置属性
			model.TopId = 42;
			model.TopName = "test";
			if(model.TopId != 42 || model.TopName != "test"){
				throw new Exception("Properties were not set correctly");
			}

			return NIL;
		});

		R("MkInst_Creates_GeneralNsDerivedModel", async(o)=>{
			var sut = NewSut();
			var dict = sut.Type_InstMkr;

			if(!dict.TryGetValue(typeof(GeneralNsDerivedModel), out var mkr)){
				throw new Exception($"No InstMkr for {nameof(GeneralNsDerivedModel)}");
			}

			var inst = mkr.MkInst();
			if(inst is not GeneralNsDerivedModel model){
				throw new Exception($"MkInst returned {inst?.GetType().Name ?? "null"}, expected GeneralNsDerivedModel");
			}

			// 验证可以访问继承的属性
			model.BaseId = 100;
			model.Age = 25;
			if(model.BaseId != 100 || model.Age != 25){
				throw new Exception("Inherited properties were not set correctly");
			}

			return NIL;
		});

		R("MkInst_Creates_NestedPublicModel", async(o)=>{
			var sut = NewSut();
			var dict = sut.Type_InstMkr;

			if(!dict.TryGetValue(typeof(NestedTypeContainer.NestedPublicModel), out var mkr)){
				throw new Exception($"No InstMkr for {nameof(NestedTypeContainer.NestedPublicModel)}");
			}

			var inst = mkr.MkInst();
			if(inst is not NestedTypeContainer.NestedPublicModel model){
				throw new Exception($"MkInst returned {inst?.GetType().Name ?? "null"}, expected NestedPublicModel");
			}

			model.Level = 1;
			model.Title = "nested";
			if(model.Level != 1 || model.Title != "nested"){
				throw new Exception("Nested model properties were not set correctly");
			}

			return NIL;
		});

		R("MkList_Creates_EmptyList_ForTopLevelNsModel", async(o)=>{
			var sut = NewSut();
			var dict = sut.Type_InstMkr;

			if(!dict.TryGetValue(typeof(TopLevelNsModel), out var mkr)){
				throw new Exception($"No InstMkr for {nameof(TopLevelNsModel)}");
			}

			var list = mkr.MkList();
			if(list is not System.Collections.IList ilist){
				throw new Exception($"MkList returned {list?.GetType().Name ?? "null"}, expected IList");
			}

			// 验证列表是空的
			if(ilist.Count != 0){
				throw new Exception($"Created list is not empty, Count={ilist.Count}");
			}

			return NIL;
		});

		R("MkList_ListElementType_Matches_TargetType", async(o)=>{
			var sut = NewSut();
			var dict = sut.Type_InstMkr;

			// 测试 GeneralNsDerivedModel 的列表
			if(!dict.TryGetValue(typeof(GeneralNsDerivedModel), out var mkr)){
				throw new Exception($"No InstMkr for {nameof(GeneralNsDerivedModel)}");
			}

			var list = mkr.MkList();
			var listType = list.GetType();
			var genericArgs = listType.GetGenericArguments();

			if(genericArgs.Length != 1){
				throw new Exception($"List type has {genericArgs.Length} generic arguments, expected 1");
			}

			if(genericArgs[0] != typeof(GeneralNsDerivedModel)){
				throw new Exception($"List element type is {genericArgs[0].Name}, expected GeneralNsDerivedModel");
			}

			return NIL;
		});

		R("MkList_ListElementType_MatchesNested_Type", async(o)=>{
			var sut = NewSut();
			var dict = sut.Type_InstMkr;

			// 测试嵌套类型的列表
			if(!dict.TryGetValue(typeof(NestedTypeContainer.NestedPublicModel), out var mkr)){
				throw new Exception($"No InstMkr for {nameof(NestedTypeContainer.NestedPublicModel)}");
			}

			var list = mkr.MkList();
			var listType = list.GetType();
			var expectedGenericArg = typeof(NestedTypeContainer.NestedPublicModel);
			var genericArgs = listType.GetGenericArguments();

			if(genericArgs.Length != 1){
				throw new Exception($"List type has {genericArgs.Length} generic arguments, expected 1");
			}

			if(genericArgs[0] != expectedGenericArg){
				throw new Exception($"List element type is {genericArgs[0].Name}, expected {expectedGenericArg.Name}");
			}

			return NIL;
		});

		R("IInstMkr_Target_Property_IsSet", async(o)=>{
			var sut = NewSut();
			var dict = sut.Type_InstMkr;

			foreach(var kvp in dict){
				var type = kvp.Key;
				var mkr = kvp.Value;

				if(mkr.Target != type){
					throw new Exception($"IInstMkr.Target ({mkr.Target.Name}) does not match dictionary key ({type.Name})");
				}
			}

			return NIL;
		});
	}
}
