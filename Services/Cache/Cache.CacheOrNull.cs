using System;
using System.Threading.Tasks;

namespace Olive.ApiClient.Services
{
    public class CacheOrNull : ICache
    {
        public async Task Run<TResponse>(RemoteSource<TResponse> remoteSource, Action<TResponse> refresher)
        {
            if (await remoteSource.ReadCache() == false)
                remoteSource.Latest.Value = default;
        }
    }
}
