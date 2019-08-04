using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using datumation_products.Shared.Infrastructure.Caching;
using datumation_products.Shared.Infrastructure.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace datumation_products.Server.Services {
    public class FileModel {
        public byte[] FileData { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
    public interface IFileService {
        Task<byte[]> GetFileData (int itemId, string fileName, string contentTyp);
        Task<string> ReadFile (Stream httpBody, string callerName);
    }

    public class FileCacheService : IFileService {

        private ShoppingCartService _shopService;
        private ICacheProvider _cacheProvider;
        public FileCacheService ([FromServices] ShoppingCartService shopService, [FromServices] ICacheProvider cacheProvider) {
            _shopService = shopService;
            _cacheProvider = cacheProvider;
        }
        private async Task<byte[]> DownloadFile (string url) {
            using (var client = new HttpClient ()) {

                using (var result = await client.GetAsync (url)) {
                    if (result.IsSuccessStatusCode) {
                        return await result.Content.ReadAsByteArrayAsync ();
                    }

                }
            }
            return null;
        }
        private async Task<string> ReadFileStream (Stream httpBody) {
            int length = 0;
            string result;
            Console.WriteLine (" File reading is stating");
            using (StreamReader reader = new StreamReader (httpBody)) {
                // Reads all characters from the current position to the end of the stream asynchronously
                // and returns them as one string.
                string s = await reader.ReadToEndAsync ();

                length = s.Length;
                result = s;
            }
            Console.WriteLine (" File reading is completed");
            return result;
        }
        public async Task<string> ReadFile (Stream httpBody, string callerName) {
            string cacheKey = $"READ_FILE_STREAM_{callerName}";
            string item = "";
            try {
                item = _cacheProvider.Retrieve<string> (cacheKey);
            } catch (System.Exception e) {

            }
            if (item == null) {
                item = await ReadFileStream (httpBody);

                if (item != null) {
                    _cacheProvider.Store (cacheKey.ToString (), item);
                }
            }
            return item;
        }
        private async Task<byte[]> GetFileDataContent (int itemId, string fileName, string contentType) {

            var net = new System.Net.WebClient ();
            var data = await DownloadFile (fileName);
            // var content = new System.IO.MemoryStream(data);

            return data;
        }
        public async Task<byte[]> GetFileData (int itemId, string fileName, string contentTyp) {

            StringBuilder cacheKey = new StringBuilder ();
            cacheKey.Append (ConfigurationFactory.Instance.Configuration ().AppSettings.AppConfiguration.CacheKeys.CacheKeyFileDownloadBase)
                .Append ("_")
                .Append ($"ITEMID_{itemId}");
            byte[] items = default;
            try {
                items = _cacheProvider.Retrieve<byte[]> (cacheKey.ToString ());
            } catch (System.Exception e) {

                Console.WriteLine ($"ERROR GET FILE DATA: {e.Message}");
            }

            if (items == null) {
                items = await GetFileDataContent (itemId, fileName, contentTyp);

                if (items != null) {
                    _cacheProvider.Store (cacheKey.ToString (), items);
                }
            }
            return items;

        }
    }

    public class FileService : IFileService {
        private ShoppingCartService _shopService;
        public FileService ([FromServices] ShoppingCartService shopService) {
            _shopService = shopService;
        }
        private async Task<byte[]> DownloadFile (string url) {
            using (var client = new HttpClient ()) {

                using (var result = await client.GetAsync (url)) {
                    if (result.IsSuccessStatusCode) {
                        return await result.Content.ReadAsByteArrayAsync ();
                    }

                }
            }
            return null;
        }
        private async Task<string> ReadFileStream (Stream httpBody) {
            int length = 0;
            string result;
            Console.WriteLine (" File reading is stating");
            using (StreamReader reader = new StreamReader (httpBody)) {
                // Reads all characters from the current position to the end of the stream asynchronously
                // and returns them as one string.
                string s = await reader.ReadToEndAsync ();

                length = s.Length;
                result = s;
            }
            Console.WriteLine (" File reading is completed");
            return result;
        }
        public async Task<byte[]> GetFileData (int itemId, string fileName, string contentType) {
            var net = new System.Net.WebClient ();
            var data = await DownloadFile (fileName);
            // var content = new System.IO.MemoryStream(data);
            return data;
        }
        public Task<string> ReadFile (Stream httpBody, string callerName) {
            return ReadFileStream (httpBody);
        }

    }
}