// Generated by https://quicktype.io

namespace datumation_products.Shared.StartupModels {
    using Newtonsoft.Json;

    public partial class LiveReload {
        [JsonProperty ("LiveReloadEnabled")]
        public bool LiveReloadEnabled { get; set; }

        [JsonProperty ("ClientFileExtensions")]
        public string ClientFileExtensions { get; set; }

        [JsonProperty ("ServerRefreshTimeout")]
        public long ServerRefreshTimeout { get; set; }

        [JsonProperty ("WebSocketUrl")]
        public string WebSocketUrl { get; set; }

        [JsonProperty ("WebSocketHost")]
        public string WebSocketHost { get; set; }

        [JsonProperty ("FolderToMonitor")]
        public string FolderToMonitor { get; set; }
    }
}