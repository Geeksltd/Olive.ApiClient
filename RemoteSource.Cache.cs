using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olive.ApiClient
{
    partial class RemoteSource<TResponse>
    {
        static object CacheSyncLock = new();
        DirectoryInfo CacheFolder;

        protected RemoteSource()
        {
            var type = typeof(TResponse);
            type = type.GetGenericArguments().SingleOrDefault() ?? type;
            var folderKey = type.Name.Replace("[]", "");
            CacheFolder = CacheRoot.GetOrCreateSubDirectory(folderKey).EnsureExists();

            Latest.Value = new StoredBindable<TResponse>(CacheFile).Value;
        }

        /// <summary>
        /// The default cache choice is Accept.         
        /// </summary>
        protected virtual CacheChoice Cache => CacheChoice.Accept;

        FileInfo CacheFile => CacheFolder.GetFile(Url.ToSimplifiedSHA1Hash() + ".json");
    }
}
