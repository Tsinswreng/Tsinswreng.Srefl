namespace Tsinswreng.Srefl;

using System;
using Tsinswreng.CsCore;


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


