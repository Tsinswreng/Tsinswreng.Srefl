namespace Tsinswreng.Srefl;

using System;
using Tsinswreng.CsCore;


[Doc($$"""
#Sum[Attribute to register type for source generator.]
#Descr[
For registered types, we can use `{{nameof(IPropAccessor)}}`
Its name must be `{{nameof(SreflType)}}`, don't change it into `{{nameof(SreflType)}}Attribute`
]
#Examples([
```cs
[{{nameof(SreflType)}}(MyClassA)]
[{{nameof(SreflType)}}(MyClassB)]
public partial class MyAppStrAcc{
	
}
```

then the source generator will add 
```cs
public {{nameof(IPropAccessorReg)}} PropAccessorReg{get;set;}
public {{nameof(IInstMkrReg)}} InstMkrReg{get;set;}
```
to `MyAppStrAcc`
])
""")]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public partial class SreflType:Attribute{
	
	public Type TargetType { get; }
	public SreflType(Type TargetType){
		this.TargetType = TargetType;
	}
}


