using System.Text.Json.Serialization;

namespace StockApp.Application.DTO
{
    public class FinnhubStockDataResponse
    {
        [JsonPropertyName("c")]
        public List<double>? ClosePrices { get; set; }

        [JsonPropertyName("h")]
        public List<double>? HighPrices { get; set; }

        [JsonPropertyName("l")]
        public List<double>? LowPrices { get; set; }

        [JsonPropertyName("o")]
        public List<double>? OpenPrices { get; set; }

        [JsonPropertyName("t")]
        public List<long>? Timestamps { get; set; }

        [JsonPropertyName("v")]
        public List<double>? Volumes { get; set; }

        [JsonPropertyName("s")]
        public string? Status { get; set; }
    }
}
