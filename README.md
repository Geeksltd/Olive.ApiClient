# Olive.ApiClient

Imagine you have the following class in your mobile project:
```csharp
public class Category
{
    public Guid ID {get; set;}
    public string Name {get; set;}
}
```
Imagine that you also have a WebAPI from which to get the latest list of categories:
```cshrap
[HttpGet("categories")]
public async Task<Category[]> GetAll()
{
  // ...
}
```

This is how you can define 

```csharp
public class CategoriesSource : RemoteSource<Category[]>
{
    
}
```

To access it, use a singleton instance:
```csharp
...
```
