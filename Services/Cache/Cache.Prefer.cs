using System;
using System.Threading.Tasks;

namespace Olive.ApiClient.Services
{
    public class Prefer : ICache
    {
        public async Task Run<TResponse>(RemoteSource<TResponse> remoteSource, Action<TResponse> refresher)
        {
            if (await remoteSource.ReadCache() == false)
                await remoteSource.TryRefresh();
        }
    }
}
