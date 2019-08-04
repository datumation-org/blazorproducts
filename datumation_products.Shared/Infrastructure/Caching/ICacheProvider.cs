namespace datumation_products.Shared.Infrastructure.Caching
{
    public interface ICacheProvider
    {
        void Remove(string key);

        T Retrieve<T>(string key);

        void Store(string key, object data);

        void Store(string key, object data, int cacheTime);

        void Store(string key, object data, int cacheTime, int timeTypeId);

        //void StoreAsCompressed(string key, object data);
        //T DecompressAndDeserialize<T>(string key);
    }
}