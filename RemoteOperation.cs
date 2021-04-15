using Olive.ApiClient.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Olive.ApiClient
{
    public class RemoteOperation
    {
        public static ApiService ApiService { get; set; } = new ApiService();
        public static DirectoryInfo CacheRoot { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).AsDirectory().GetOrCreateSubDirectory("_apiCache");
        public static string GetTypeName<T>() => typeof(T).GetGenericArguments().SingleOrDefault()?.Name ?? typeof(T).Name.Replace("[]", "");
    }
}
