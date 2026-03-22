using Tsinswreng.Srefl;


namespace Tsinswreng.Srefl.Test.Models{
	/// 這組類型專供 IPropAccessor / IPropAccessorReg 測試共用。
	/// 目標是把「屬性、字段、方法、繼承、嵌套類、不同命名空間位置」都覆蓋到。
	public class GeneralNsBaseModel{
		public int BaseId { get; set; }
		public int PublicBaseField;
		protected int ProtectedBaseProp { get; set; }
		private int PrivateBaseProp { get; set; }

		public int ReadOnlyBaseProp { get; } = 7;
		public int WriteOnlySink { get; private set; }
		public int WriteOnlyBaseProp {
			set => WriteOnlySink = value;
		}

		public str BaseMethod(){
			return "base";
		}
	}

	public class GeneralNsDerivedModel: GeneralNsBaseModel{
		public int Age { get; set; }
		public string? Name { get; set; }
		public decimal Score { get; set; }

		public int PublicField;
		private string? PrivateProp { get; set; }

		public str DerivedMethod(){
			return "derived";
		}
	}

	public class NestedTypeContainer{
		public class NestedPublicModel{
			public int Level { get; set; }
			public string? Title { get; set; }
			public int PublicField;
			private int PrivateNestedProp { get; set; }

			public str Echo(){
				return "nested";
			}
		}

		internal class NestedInternalModel{
			public Guid Id { get; set; }
			public int Count { get; set; }
		}
	}
}


namespace Tsinswreng.Srefl.Test{
	/// 這個類型在測試項目頂級命名空間下，用來覆蓋「頂級命名空間類型」場景。
	public class TopLevelNsModel{
		public int TopId { get; set; }
		public string? TopName { get; set; }
		public int TopField;

		public str TopMethod(){
			return "top";
		}
	}

	/// 使用 StrAccType 收編測試模型，供源生成器識別。
	[StrAccType(typeof(TopLevelNsModel))]
	[StrAccType(typeof(Models.GeneralNsBaseModel))]
	[StrAccType(typeof(Models.GeneralNsDerivedModel))]
	[StrAccType(typeof(Models.NestedTypeContainer.NestedPublicModel))]
	[StrAccType(typeof(Models.NestedTypeContainer.NestedInternalModel))]
	public partial class SharedModelStrAccRegistry{
		public static SharedModelStrAccRegistry Inst=>field??=new SharedModelStrAccRegistry();
	}
}
