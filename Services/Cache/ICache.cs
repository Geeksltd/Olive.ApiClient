using System;
using System.Threading.Tasks;

namespace Olive.ApiClient.Services
{
    public interface ICache
    {
        public Task Run<TResponse>(RemoteSource<TResponse> remoteSource, Action<TResponse> refresher);
    }
}