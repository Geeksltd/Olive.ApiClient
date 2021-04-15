using System;

namespace Olive.ApiClient.Services
{
    public class CacheFactory
    {
        private CacheFactory() { }
        public static ICache Create(CacheChoice choice)
        {
            switch (choice)
            {
                case CacheChoice.Prefer: return new Prefer();
                case CacheChoice.Accept: return new Accept();
                case CacheChoice.Refuse: return new Refuse();
                case CacheChoice.CacheOrNull: return new CacheOrNull();
                case CacheChoice.AcceptButWarn: return new AcceptButWarn();
                case CacheChoice.PreferThenUpdate: return new PreferThenUpdate();
                default: throw new ArgumentException($"Cache factory is not able to create {choice}.");
            }
        }
    }

}
