using System.Threading.Tasks;

namespace Olive.ApiClient
{
    public abstract class RemoteSource<TParam, TResponse> : RemoteSource
    {
        protected abstract string GetUrl(TParam arg);

        public async Task<RemoteSource<TResponse>> For(TParam arg)
        {
            var remoteSource = new ParameterisedRemoteSource<TResponse>(GetUrl(arg));
            return await remoteSource.Load();
        }
    }

    public class ParameterisedRemoteSource<TResponse> : RemoteSource<TResponse>
    {
        private readonly string ParameterisedUrl;
        public ParameterisedRemoteSource(string parameterisedUrl) => ParameterisedUrl = parameterisedUrl;
        protected override string Url => ParameterisedUrl;
        protected override CacheChoice Cache => CacheChoice.Prefer;
    }
}
