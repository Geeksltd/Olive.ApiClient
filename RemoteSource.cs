using Newtonsoft.Json;
using Olive.ApiClient.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Olive.ApiClient
{
    public class RemoteSource : RemoteOperation
    {
        protected async Task<TResponse> FetchData<TResponse>(string url, FileInfo cacheFile)
        {
            var apiResult = await ApiService.Get<TResponse>(url);

            if (cacheFile.Exists())
            {
                string localCachedVersion = (await cacheFile.ReadAllTextAsync()).CreateSHA1Hash();

                if (localCachedVersion.IsEmpty()) throw new Exception("Local cached file's hash is empty!");

                var newResponseCache = JsonConvert.SerializeObject(apiResult).CreateSHA1Hash();

                if (newResponseCache == localCachedVersion)
                    throw new Exception("Same response. No update needed!");
            }
            return apiResult;
        }
    }

    public abstract partial class RemoteSource<TResponse> : RemoteSource
    {
        /// <summary>
        /// Holds the latest result of the API Call. 
        /// It's normally instantiated from the latest cache and then updated with every Refresh attempt.
        /// </summary>
        public readonly Bindable<TResponse> Latest = new Bindable<TResponse>();

        /// <summary>
        /// Raised when any refresh attempt fails.
        /// </summary>
        public event Action<Exception> Failed;

        /// <summary>
        /// Will attempt a refresh, but will not throw an error in case of an exception.
        /// If it was successful and the response was different from the latest cache, updates the value of Latest
        /// </summary>
        public virtual async Task<bool> TryRefresh()
        {
            try
            {
                var result = await FetchData<TResponse>(Url, CacheFile);
                return await PersistCache(result);
            }
            catch (Exception ex)
            {
                Failed?.Invoke(ex);
                return false;
            }
        }

        /// <summary>
        /// Will attempt to refresh. It throws an exception if failed.
        /// Call this in a try/catch block, or use TryRefresh() instead.
        /// </summary>
        public async virtual Task RefreshOrFail()
        {
            var result = await FetchData<TResponse>(Url, CacheFile);
            await PersistCache(result);
        }

        /// <summary>
        /// The URL to send the http request to.
        /// </summary>
        protected abstract string Url { get; }

        /// <summary>
        /// The default cache choice is Accept.         
        /// </summary>
        protected virtual CacheChoice Cache => CacheChoice.Accept;
    }
}
