using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Olive.ApiClient.Services;

namespace Olive.ApiClient
{
    public partial class RemoteSource<TResponse>
    {
        static object CacheSyncLock = new();
        public ICache CacheStrategy;
        protected DirectoryInfo CacheFolder;

        public RemoteSource()
        {
            CacheFolder = CacheRoot.GetOrCreateSubDirectory(GetTypeName<TResponse>()).EnsureExists();
            CacheStrategy = CacheFactory.Create(Cache);
        }

        public async Task<RemoteSource<TResponse>> Load(Action<TResponse> refresher = null)
        {
            if (refresher != null && Cache != CacheChoice.PreferThenUpdate)
                throw new ArgumentException("Refresher can only be provided when using ApiResponseCache.PreferThenUpdate.");

            if (refresher == null && Cache == CacheChoice.PreferThenUpdate)
                throw new ArgumentException("When using ApiResponseCache.PreferThenUpdate, refresher must be specified.");

            await CacheStrategy.Run(this, refresher);
            return this;
        }

        protected FileInfo CacheFile
        {
            get
            {
                lock (CacheSyncLock)
                    return CacheFolder.GetFile(Url.ToSimplifiedSHA1Hash() + ".json");
            }
        }

        public async Task<bool> ReadCache()
        {
            if (CacheFile.Exists())
            {
                var content = await CacheFile.ReadAllTextAsync();
                if (content.CreateSHA1Hash().HasValue())
                {
                    Latest.Value = new StoredBindable<TResponse>(CacheFile);
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> PersistCache(TResponse response)
        {
            if (response == null)
                return false;

            await CacheFile.WriteAllTextAsync(JsonConvert.SerializeObject(response));
            Latest.Value = new StoredBindable<TResponse>(CacheFile);
            return true;
        }
    }
}
