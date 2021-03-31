using System;
using System.IO;

namespace Olive.ApiClient
{
    public class RemoteOperation
    {
        public static DirectoryInfo CacheRoot { get; set; }
            = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).AsDirectory().GetOrCreateSubDirectory("_apiCache");
    }
}
