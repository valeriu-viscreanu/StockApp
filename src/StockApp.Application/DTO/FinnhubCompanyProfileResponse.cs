using System.Text.Json.Serialization;

namespace StockApp.Application.DTO
{
    public class FinnhubCompanyProfileResponse
    {
        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("exchange")]
        public string? Exchange { get; set; }

        [JsonPropertyName("ipo")]
        public string? Ipo { get; set; }

        [JsonPropertyName("marketCapitalization")]
        public double? MarketCapitalization { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("shareOutstanding")]
        public double? ShareOutstanding { get; set; }

        [JsonPropertyName("ticker")]
        public string? Ticker { get; set; }

        [JsonPropertyName("weburl")]
        public string? WebUrl { get; set; }

        [JsonPropertyName("logo")]
        public string? Logo { get; set; }

        [JsonPropertyName("finnhubIndustry")]
        public string? FinnhubIndustry { get; set; }
    }
}
