using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olive.ApiClient
{
    public abstract partial class RemoteSource<TResponse> : RemoteOperation
    {
        /// <summary>
        /// Holds the latest result of the API Call. 
        /// It's normally instantiated from the latest cache and then updated with every Refresh attempt.
        /// </summary>
        public readonly Bindable<TResponse> Latest = new Bindable<TResponse>();

        /// <summary>
        /// Raised when any refresh attempt fails.
        /// </summary>
        public event Action<Exception> Failed;

        /// <summary>
        /// Will attempt a refresh, but will not throw an error in case of an exception.
        /// If it was successful and the response was different from the latest cache, Latest the value of 
        /// </summary>
        public virtual void TryRefresh()
        {
        }

        /// <summary>
        /// Will attempt to refresh. It throws an exception if failed.
        /// Call this in a try/catch block, or use TryRefresh() instead.
        /// </summary>
        public virtual Task RefreshOrFail()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The URL to send the http request to.
        /// </summary>
        protected abstract string Url { get; }

    }
}
