namespace Olive.ApiClient
{
    public enum OnError
    {
        /// <summary>
        //  Will throw the error. If you use this, ensure to have a try-catch somewhere in
        //  the call chain.
        /// </summary>
        Throw = 0,
        /// <summary>
        //  Will not throw an error in case of an exception.
        /// </summary>
        Ignore = 1,
        /// <summary>
        /// Not implemented yet.
        /// </summary>
        TryLater = 1,
    }
}
