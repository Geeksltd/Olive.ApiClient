using Olive.ApiClient.Services;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace Olive.ApiClient
{
    public class RemoteOperation
    {
        protected static ApiService ApiService { get; set; } = new ApiService();
        protected static DirectoryInfo CacheRoot { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).AsDirectory().GetOrCreateSubDirectory("_apiCache");
        protected static string GetTypeName<T>() => typeof(T).GetGenericArguments().SingleOrDefault()?.Name ?? typeof(T).Name.Replace("[]", "");
        protected static T GetDefault<T>() => typeof(T).IsArray ? (T)Activator.CreateInstance(typeof(T), 0) : default(T);
        public static void ProvideToken(Func<string> tokenProvider)
        {
            var sessionToken = tokenProvider.Invoke();
            if (sessionToken.HasValue())
                ApiService.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionToken);
        }
    }
}
