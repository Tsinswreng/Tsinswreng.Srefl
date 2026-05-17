// #import "@preview/tsinswreng-auto-heading:0.1.0": auto-heading
// #let H = auto-heading;

// #H[Tsinswreng.Srefl][
// 	Tsinswreng.Srefl 是一組“結構化反射”相關抽象。

// 	它更偏向：

// 	- 屬性訪問器抽象
// 	- 對象實例構造器抽象
// 	- 字典序列化/反序列化接口
// 	- 配合註冊表或源生成器的映射協議

// 	#H[安裝][
// 		```bash
// 		dotnet add package Tsinswreng.Srefl --version 0.0.1-alpha
// 		```
// 	]
// ]
````md
# Srefl
Convert between `IDictionary<string,object?>` and C# object.

AOT friendly, Free of reflection.

**⚠️This project has no release version yet and everything may change. If you want to use it, we recommand you to clone the code and reference the source code directly.⚠️**

# Usage

## Config the mapper

```cs
using Tsinswreng.Srefl;

namespace MyProjectNamespace; // don't put things in global namespace otherwise it may not work

public partial class Person{
	public int Age{get;set;}
	public string Name {get;set;}
}

[DictType(typeof(Person))]
//[DictType(typeof(MoreTypes))]
public partial class AppDictMapper{
	public static AppDictMapper Inst{get;} = new();

}
```

## Use the mapper

```cs
using Tsinswreng.Srefl;
// object to dictionary:
var person = new Person{Age=22, Name="Tsinswreng"};
var dict = AppDictMapper.Inst.ToDictShallowT(person);
// the dict will be like {"Age":22, "Name":"Tsinswreng"}

// assign the dictionary to object:
var person2 = new Person();
AppDictMapper.Inst.AssignShallowT(person2, dict);
// the content of `person2` will be the same as `person`

```

# Reference this project by source code
+ Clone the repo
+ In your .csproj file:
```xml
<ItemGroup>
	<!-- reference the source generator assembly -->
	<ProjectReference Include="../Tsinswreng.Srefl/proj/Tsinswreng.Srefl.SrcGen/Tsinswreng.Srefl.SrcGen.csproj"
		OutputItemType="Analyzer" ReferenceOutputAssembly="false"
	/>

	<!-- reference the API assembly, including Attributes, interfaces etc. -->
	<ProjectReference Include="../Tsinswreng.Srefl/proj/Tsinswreng.Srefl/Tsinswreng.Srefl.csproj" />

</ItemGroup>
```

# Documentation

The class attached `[DictType(typeof(T))]` will automatically implement the interface `IDictMapperShallow`


```cs
namespace Tsinswreng.Srefl;
using str = string;
public  partial interface IDictMapperShallow{
	public IDictionary<Type, IDictMapperForOneType> Type_Mapper{get;set;}
	public IDictionary<str, object?> ToDictShallowT<T>(T Obj);
	public IDictionary<str, object?> ToDictShallow(Type Type, object? Obj);

	public IDictionary<str, Type> GetTypeDictShallowT<T>();
	public IDictionary<str, Type> GetTypeDictShallow(Type Type);

	public T AssignShallowT<T> (T Obj, IDictionary<str, object?> Dict);
	public object AssignShallow(Type Type, object? Obj, IDictionary<str, object?> Dict);
}


public  partial interface IDictMapperForOneType{
	public Type TargetType{get;}
	public IDictionary<str, object?> ToDictShallow(object Obj);
	public IDictionary<str, Type> GetTypeDictShallow();
	public object AssignShallow(object Obj, IDictionary<str, object?> Dict);

}


```

````
