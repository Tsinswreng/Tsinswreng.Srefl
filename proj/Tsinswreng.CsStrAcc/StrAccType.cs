namespace Tsinswreng.CsStrAcc;

using System;
using Tsinswreng.CsCore;


/// 名須潙DictType、叵作DictTypeAttribute
/// 緣用及nameof、斯類ʹ名ˋ 須同於 作特性ⁿ引用旹厎
/// 如引用時用[DictType(...)]則斯類ʹ名則須潙DictType、叵作DictTypeAttribute
[Doc($$"""
#Sum[Attribute to register type for source generator.]
#Descr[
For registered types, we can use `{{nameof(IPropAccessor)}}`
Its name must be `{{nameof(StrAccType)}}`, don't change it into `{{nameof(StrAccType)}}Attribute`
]
#Examples([
```cs
[{{nameof(StrAccType)}}(MyClassA)]
[{{nameof(StrAccType)}}(MyClassB)]
public partial class MyAppStrAcc{
	
}
```

then the source generator will add 
```cs
public {{nameof(IPropAccessorMgr)}} PropAccessorMgr{get;set;}
```
to `MyAppStrAcc`
])
""")]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public partial class StrAccType:Attribute{
	
	public Type TargetType { get; }
	public StrAccType(Type TargetType){
		this.TargetType = TargetType;
	}
}


