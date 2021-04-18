namespace Olive.ApiClient
{

    public abstract class RemoteSource<TParam, TResponse>
    {
        protected abstract bool WarnOnFailure { get; }
        protected abstract string GetUrl(TParam arg);
        public RemoteSource<TResponse> For(TParam arg) => new ParameterisedRemoteSource<TResponse>(GetUrl(arg), WarnOnFailure);
    }

    public class ParameterisedRemoteSource<TResponse> : RemoteSource<TResponse>
    {
        private readonly string _ParameterisedUrl;
        private readonly bool _WarnOnFailure;

        public ParameterisedRemoteSource(string parameterisedUrl, bool warnOnFailure) : base(isParameterised: true)
        {
            _ParameterisedUrl = parameterisedUrl;
            _WarnOnFailure = warnOnFailure;
            ReadCache();
        }

        protected override string Url => _ParameterisedUrl;
        protected override bool WarnOnFailure => _WarnOnFailure;
    }

}
