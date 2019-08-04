using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace datumation_products.Shared.ViewModels

{
    public partial class ChargePaymentModel {
        [JsonProperty ("chargeModel")]
        public ChargeModel ChargeModel { get; set; }
    }

    public partial class ChargeModel {
        [JsonProperty ("token")]
        public string Token { get; set; }

        [JsonProperty ("email")]
        public string Email { get; set; }

        [JsonProperty ("user")]
        public string User { get; set; }

        [JsonProperty ("amount")]
        public long? Amount { get; set; }

        [JsonProperty ("product")]
        public string Product { get; set; }

        [JsonProperty ("description")]
        public string Description { get; set; }
    }

    public partial class ChargePaymentModel {
        public static ChargePaymentModel FromJson (string json) =>
            JsonConvert.DeserializeObject<ChargePaymentModel> (json, Converter.Settings);
    }

    public static class Serialize {
        public static string ToJson (this ChargePaymentModel self) => JsonConvert
            .SerializeObject (self, Converter.Settings);
    }

    internal static class Converter {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}