# EfNavGuard

C# source generator to generate guards for Entity Framework Core required navigation properties. Basically, it generates a boilerplate code presented in https://learn.microsoft.com/en-us/ef/core/miscellaneous/nullable-reference-types#required-navigation-properties.

> [!WARNING]  
> To use this generator, your project must target .NET 9.0 or newer.

## Installation

- Modify your .csproj file by adding new `PackageReference`:

```xml
<ItemGroup>
    <PackageReference Include="EfNavGuard" Version="0.1.0" OutputItemType="Analyzer" ReferenceOutputAssembly="false" />
</ItemGroup>
```

## Usage

Consider the following model:

```csharp
public class MainEntity
{
    public int Id { get; init; }
    
    public int OtherEntityId { get; init; }
    public OtherEntity OtherEntity { get; set; }
}

public class OtherEntity
{
    public int Id { get; set; }
}
```

Above code will produce warning if project has NRT enabled. To fix it with `EfNavGuard`, you need to apply following changes:

- Mark `MainEntity` class and `OtherEntity` property as partial (this is required, so the source generator can augment the class).
- Apply `GuardedNavigation` attribute to `OtherEntity` property.

So the final code will look like this:

```csharp
using EfNavGuard;

public partial class MainEntity
{
    public int Id { get; init; }
    
    public int OtherEntityId { get; init; }
    [GuardedNavigation]
    public partial OtherEntity OtherEntity { get; set; }
}

public class OtherEntity
{
    public int Id { get; set; }
}
```

Now the warning is gone. And if someone tries to access `OtherEntity` property without loading it, exception will be thrown.