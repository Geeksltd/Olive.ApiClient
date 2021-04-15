using System;
using System.Threading.Tasks;

namespace Olive.ApiClient.Services
{
    public class Refuse : ICache
    {
        public async Task Run<TResponse>(RemoteSource<TResponse> remoteSource, Action<TResponse> refresher) => await remoteSource.TryRefresh();
    }
}
