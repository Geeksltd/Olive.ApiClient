using Olive.ApiClient.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Olive.ApiClient
{
    public abstract class RemoteCommand<TResponse> : RemoteOperation
    {
        /// <summary>
        /// The URL to send the http request to.
        /// </summary>
        protected abstract string Url { get; }

        /// <summary>
        // The http method that will be used to send the http request with.
        /// </summary>
        protected virtual HttpMethod HttpMethod => HttpMethod.Post;

        /// <summary>
        /// Will attempt to send the http request and return the response.
        /// </summary>
        public Task<TResponse> Run(object param)
        {
            if (OnError == OnError.Throw)
                return SendOrFail(param);

            if (OnError == OnError.Ignore)
                return TrySend(param);

            if (OnError == OnError.TryLater)
                throw new NotImplementedException();

            return TrySend(param);
        }

        private Task<TResponse> SendOrFail(object param) => ApiService.Send<TResponse>(this.Url, HttpMethod, param);

        private async Task<TResponse> TrySend(object param)
        {
            try
            {
                return await ApiService.Send<TResponse>(Url, HttpMethod, param);
            }
            catch
            {
                return default;
            }
        }

        protected virtual OnError OnError => OnError.Throw;

    }
}
