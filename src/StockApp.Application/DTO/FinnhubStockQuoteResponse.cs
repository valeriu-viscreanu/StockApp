using System.Text.Json.Serialization;

namespace StockApp.Application.DTO
{
    public class FinnhubStockQuoteResponse
    {
        [JsonPropertyName("c")]
        public double CurrentPrice { get; set; }

        [JsonPropertyName("d")]
        public double Change { get; set; }

        [JsonPropertyName("dp")]
        public double PercentChange { get; set; }

        [JsonPropertyName("h")]
        public double HighPriceDay { get; set; }

        [JsonPropertyName("l")]
        public double LowPriceDay { get; set; }

        [JsonPropertyName("o")]
        public double OpenPriceDay { get; set; }

        [JsonPropertyName("pc")]
        public double PreviousClosePrice { get; set; }

        [JsonPropertyName("t")]
        public long Timestamp { get; set; }
    }
}
