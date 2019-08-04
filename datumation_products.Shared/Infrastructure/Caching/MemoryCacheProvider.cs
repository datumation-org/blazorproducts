using System;
using System.Runtime.Caching;
using datumation_products.Shared.Infrastructure.Caching;

namespace datumation_products.Shared.Infrastructure.Caching
{
    public class MemoryCacheProvider : ICacheProvider
    {
        private ObjectCache Cache { get { return MemoryCache.Default; } }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        public void Store(string key, object data)
        {
            Cache.Add(key, data, ObjectCache.InfiniteAbsoluteExpiration);
        }

        public void Store(string key, object data, int cacheTime)
        {
            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime) };

            Cache.Add(new CacheItem(key, data), policy);
        }

        public void Store(string key, object data, int cacheTime, int timeTypeId)
        {
            var policy = new CacheItemPolicy { };

            //if (timeTypeId == 2)
            //{
            // policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(cacheTime);
            //}
            //else if (timeTypeId == 3)
            //{
            //  policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMilliseconds(cacheTime);
            //}
            // else
            //{
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            //}

            Cache.Add(new CacheItem(key, data), policy);
        }

        public T Retrieve<T>(string key)
        {
            var itemStored = (T)Cache.Get(key);

            if (itemStored == null)
                itemStored = default(T);

            return itemStored;
        }
    }
}