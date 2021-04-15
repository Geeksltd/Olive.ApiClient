using System;
using System.Threading.Tasks;

namespace Olive.ApiClient.Services
{
    public class PreferThenUpdate : ICache
    {
        public async Task Run<TResponse>(RemoteSource<TResponse> remoteSource, Action<TResponse> refresher)
        {
            if (await remoteSource.ReadCache())
                Task.Run(new Action(() => remoteSource.TryRefresh().ContinueWith(x => refresher.Invoke(remoteSource.Latest.Value))));
            else
                await remoteSource.TryRefresh();
        }
    }
}
