# Olive API Client
This is a library to provide a simple interface for dealing with Web Apis.
It works perfectly with MvvM programming model using Bindable<....> objects.

Benefits:
- It has a simple decalarive API
- It needs minimum effort from the developer
- It's super fast and cached by default
- It's fresh and auto-updated upon receiving a simple TryRefresh signal
- It's resilient and fault tolerant


## Scneario 1: Simple Get Api

Imagine you have the following class in your mobile project:

```csharp
public class Category
{
    public Guid ID {get; set;}
    public string Name {get; set;}
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
```

In the mobile app, create a subclass of `RemoteSource` for each Web Api action.

```csharp
public class CategoriesSource : RemoteSource<Category[]>
{
    protected override string Url => "/api/categories";
}
```

Define a static class named `Api` in your mobile app:

```csharp
static class Api
{
    public static readonly CategoriesSource Categories = new();
    // ...
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
         // The page will load instantly from the latest cache. No waiting.
         public Bindable<Category[]> Source => Api.Categories.Latest;
         
         protected override void OnNavigationStarted()
         {   
             // This will attempt to get fresh data from the server. 
             // If anything was changed, the normal binding engine will update the UI.              
             // No further action is required by you.
             Api.Categories.Latest.TryRefresh(); 
         }
     }
}
```
This mechanism will mean that:
 - Your UI is rendered instantly (from the latest cache).
 - If there is new data, you will get that and update the UI with no added delay
 - You get the best of performance and recency. No compromise.


## Scneario 2: Parameterised Get Api

Imagine you have the following class in your mobile project:

```csharp
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
[HttpGet("api/categories/{categoryId}/products")]
public async Task<Product[]> GetProducts(Guid categoryId)
{
   // ...
}
```

In the mobile app, create a subclass of `RemoteSource` with the argument as the first parameter, and the return type as the second.
In this example we are using a Guid parameter (the category of the products to return).
But it can also be a complex type to wrap multiple individual parameters if needed.

```csharp
public class CategoryProductsSource : RemoteSource<Guid, Product[]>
{
    protected override string GetUrl(Guid arg) => "/api/categories/" + arg;
}
```

Just like the previous example, in the `Api` class create an instance of it:

```csharp
static class Api
{
    //...
    public static readonly CategoryProductsSource CategoryProducts = new();
}
```

To use this, call the `For(...)` method with the required parameter. 
This method will create and return a unique `RemoteSource` object for each unique given argument.
For example, in your ViewModel, imagine you have a page to show the list of products for a given category.
Every time you go back to this page with a different category, a new API call will be required of course.

But if returning to it for the same category that you have visited once before (and thus have cache available),
it will render that instantly and then refresh if necessary.

```csharp
namespace ViewModel
{
     class ProductsList : FullScreen
     {
         public readonly Bindable<Category> Category = new();
                  
         public Bindable<Product[]> Source => Category.Get(c => Api.CategoryProducts.For(c.ID).Latest);
         
         protected override void OnNavigationStarted()
         {   
             Api.CategoryProducts.For(Category.Value.ID).TryRefresh(); 
         }
     }
}
```
