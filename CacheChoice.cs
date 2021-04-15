namespace Olive.ApiClient
{
    public enum CacheChoice
    {
        /// <summary>
        /// If a cache is available, that's preferred and there is no need for a fresh Web Api request.
        /// </summary>
        Prefer,

        /// <summary>
        /// If a cache is available, that's returned immediately. But a call will still be made to the server to check for an update, in which case a provided refresher delegate will be invoked.
        /// </summary>
        PreferThenUpdate,

        /// <summary>
        /// Means a new request should be sent. But if it failed and a cache is available, then that's accepted.
        /// </summary>
        Accept,

        /// <summary>
        /// A new request should be sent. But if it failed and a cache is available, then that's accepted. However a warning toast will be displayed to the user in that case to say: The latest data cannot be received from the server right now.
        /// </summary>
        AcceptButWarn,

        /// <summary>
        /// Only a fresh response from the server is acceptable, and any cache should be ignored.
        /// </summary>
        Refuse,

        /// <summary>
        /// Only a cached response will be used and a new request will not be sent.
        /// </summary>
        CacheOrNull
    }
}
