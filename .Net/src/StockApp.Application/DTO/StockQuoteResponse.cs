using System.Text.Json.Serialization;

namespace StockApp.Application.DTO
{
    public class StockQuoteResponse
    {
        /// <summary>Mid-market price, before the trading spread is applied.</summary>
        [JsonPropertyName("c")]
        public double? CurrentPrice { get; set; }

        /// <summary>Price a seller receives: mid minus half the spread.</summary>
        [JsonPropertyName("bid")]
        public double? BidPrice { get; set; }

        /// <summary>Price a buyer pays: mid plus half the spread.</summary>
        [JsonPropertyName("ask")]
        public double? AskPrice { get; set; }

        [JsonPropertyName("d")]
        public double? Change { get; set; }

        [JsonPropertyName("dp")]
        public double? PercentChange { get; set; }

        [JsonPropertyName("h")]
        public double? HighPriceDay { get; set; }

        [JsonPropertyName("l")]
        public double? LowPriceDay { get; set; }

        [JsonPropertyName("o")]
        public double? OpenPriceDay { get; set; }

        [JsonPropertyName("pc")]
        public double? PreviousClosePrice { get; set; }

        [JsonPropertyName("t")]
        public long? Timestamp { get; set; }
    }
}
