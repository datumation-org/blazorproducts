using System;
using System.Threading.Tasks;
using datumation_products.Server.Data;
using datumation_products.Shared.Infrastructure.Caching;

namespace datumation_products.Server.Services {

    public interface ICountService {
        Task<int> GetCountByState (string stateAbbr);
        Task<int> GetCountByType ();
        Task<int> GetCountByTypeWithPar (string pt);
        Task<int> GetComboCount (string state, string pt);
    }
    public class CountCacheService : ICountService {
        private ICacheProvider _cache;
        private IDataRepo _data;
        public CountCacheService (ICacheProvider cache, IDataRepo data) {
            _cache = cache;
            _data = data;
        }
        public async Task<int> GetCountByState (string stateAbbr) {
            string cacheKey = "GET_COUNT_BY_STATE_CACHE_KEY_" + stateAbbr;
            int result = 0;
            try {
                result = await _cache.Retrieve<Task<int>> (cacheKey);
                if (result == 0) {
                    result = await _data.GetCountByState (stateAbbr);
                    _cache.Store (cacheKey, result);

                }
            } catch (System.Exception) {
                result = await _data.GetCountByState (stateAbbr);
                _cache.Store (cacheKey, result);

            }
            return await Task.FromResult (result);
        }
        public async Task<int> GetComboCount (string state, string pt) {
            string cacheKey = "GET_COUNT_BY_COMBO_CACHE_KEY_" + state + "_" + pt;
            int result = 0;
            try {
                result = await _cache.Retrieve<Task<int>> (cacheKey);
                if (result == 0) {
                    result = await _data.GetComboCount (state, pt);
                    _cache.Store (cacheKey, result);

                }
            } catch (System.Exception) {
                result = await _data.GetComboCount (state, pt);
                _cache.Store (cacheKey, result);

            }
            return await Task.FromResult (result);
        }

        public async Task<int> GetCountByType () {
            string cacheKey = "GET_COUNT_BY_TYPE_CACHE_KEY";
            int result = 0;
            try {
                result = await _cache.Retrieve<Task<int>> (cacheKey);
                if (result == 0) {
                    result = await _data.GetCountByType ();
                    _cache.Store (cacheKey, result);
                }
            } catch (System.Exception) {
                result = await _data.GetCountByType ();

                _cache.Store (cacheKey, result);
            }
            return await Task.FromResult (result);
        }
        public async Task<int> GetCountByTypeWithPar (string pt) {
            string cacheKey = "GET_COUNT_BY_TYPE_CACHE_KEY_" + pt;
            int result = 0;
            try {
                result = await _cache.Retrieve<Task<int>> (cacheKey);
                if (result == 0) {
                    result = await _data.GetCountByTypeWithPar (pt);
                    _cache.Store (cacheKey, result);
                }
            } catch (System.Exception) {

                result = await _data.GetCountByTypeWithPar (pt);

                _cache.Store (cacheKey, result);
            }
            return await Task.FromResult (result);
        }
    }

}