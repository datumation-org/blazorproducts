// Generated by https://quicktype.io

namespace datumation_products.Shared.StartupModels {
    using Newtonsoft.Json;

    public partial class Stripe {
        [JsonProperty ("SecretKey")]
        public string SecretKey { get; set; }

        [JsonProperty ("PublishableKey")]
        public string PublishableKey { get; set; }
    }
}