using System;
using Newtonsoft.Json;

namespace scb10x_assignment_party_haan_backend.Domain.DTOs
{
    [JsonObject("tokenSetting")]
    public class TokenSetting
    {
        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonProperty("audience")]
        public string Audience { get; set; }

        [JsonProperty("accessExpiration")]
        public int AccessExpiration { get; set; }
    }
}
