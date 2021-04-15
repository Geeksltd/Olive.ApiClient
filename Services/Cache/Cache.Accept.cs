using System;
using System.Threading.Tasks;

namespace Olive.ApiClient.Services
{
    public class Accept : ICache
    {
        public async Task Run<TResponse>(RemoteSource<TResponse> remoteSource, Action<TResponse> refresher)
        {
            var updated = await remoteSource.TryRefresh();
            if (updated == false)
                remoteSource.ReadCache();
        }
    }
}
