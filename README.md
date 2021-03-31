# Olive.ApiClient

Imagine you have the following classes in your mobile project:

```csharp
public class Category
{
    public Guid ID {get; set;}
    public string Name {get; set;}
    // ...
}

public class Product
{
    public Guid ID {get; set;}
    public Guid CategoryId {get; set;}
    public string Brand {get; set;}
    // ...
}
```

Imagine that you also have a WebAPI from which to get the latest list of categories:

```csharp
[HttpGet("api/categories")]
public async Task<Category[]> GetAll()
{
   // ...
}

[HttpGet("api/categories/{categoryId}/products")]
public async Task<Product[]> GetProducts(Guid categoryId)
{
   // ...
}
```

In the mobile app, create a subclass of `RemoteSource` for each Web Api action.
The following shows a parameterless example *(all categories)* and also a parameterised api *(products for a given category id)*.

```csharp
public class CategoriesSource : RemoteSource<Category[]>
{
    protected override string Url => "/api/categories";
}

public class CategoryProductsSource : RemoteSource<Guid, Product[]>
{
    protected override string GetUrl(Guid arg) => "/api/categories/" + arg;
}
```

Define a static class named `Api` in your mobile app:

```csharp
static class Api
{
    public static readonly CategoriesSource Categories = new();
    public static readonly CategoryProductsSource CategoryProducts = new();
}
```

You can think of these objects as local proxies for remote api actions.
These objects offer a `Bindable` property named `Latest` which can be used like any other in-memory bindable object.

For example, in your ViewModel you can simply use them like this: 

```csharp
namespace ViewModel
{
     class CategoriesList : FullScreen
     {
         public Bindable<Category[]> Source => Api.Categories.Latest;
         
         protected override void OnNavigationStarted()
         {
             // This method will call the server to attempt getting the latest data.
             // If anything was changed, the Bindable value will simply change. Otherwise the latest cache will be used.
             // The page will load instantly from the latest page, and if new data actually arrived, the UI will update to reflect that.
             // So no action is needed by you, and yet you get the best mix of performance and up-to-date-ness possible.
             Api.Categories.Latest.TryRefresh(); 
         }
     }
}
```
