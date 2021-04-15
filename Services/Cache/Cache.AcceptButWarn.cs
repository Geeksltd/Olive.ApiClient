using System;
using System.Threading.Tasks;

namespace Olive.ApiClient.Services
{
    public class AcceptButWarn : ICache
    {
        public async Task Run<TResponse>(RemoteSource<TResponse> remoteSource, Action<TResponse> refresher)
        {
            var updated = await remoteSource.TryRefresh();
            if (updated == false)
                await remoteSource.ReadCache();
        }
    }
}
