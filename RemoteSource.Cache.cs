using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Olive.ApiClient
{
    public abstract partial class RemoteSource<TResponse>
    {
        static object CacheSyncLock = new();
        protected DirectoryInfo CacheFolder;

        public RemoteSource(bool isParameterised = false)
        {
            CacheFolder = CacheRoot.GetOrCreateSubDirectory(GetTypeName<TResponse>()).EnsureExists();
            if (isParameterised == false)
                ReadCache();
        }

        protected void ReadCache()
        {
            if (CacheFile.Exists())
            {
                var content = CacheFile.ReadAllText();
                if (content.CreateSHA1Hash().HasValue())
                    Latest.Value = new StoredBindable<TResponse>(CacheFile);
            }
            else
            {
                Latest.Value = GetDefault<TResponse>();
                if (WarnOnFailure)
                    FailureWarning?.Invoke(new Exception($"No cache file was found for {Url}."));
            }
        }

        protected async Task<bool> PersistCache(TResponse response)
        {
            if (response == null)
                return false;

            await CacheFile.WriteAllTextAsync(JsonConvert.SerializeObject(response));
            Latest.Value = new StoredBindable<TResponse>(CacheFile);
            return true;
        }

        protected virtual FileInfo CacheFile
        {
            get
            {
                lock (CacheSyncLock)
                    return CacheFolder.GetFile(Url.ToSimplifiedSHA1Hash() + ".json");
            }
        }

    }

}
