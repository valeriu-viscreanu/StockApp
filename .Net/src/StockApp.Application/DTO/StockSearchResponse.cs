using System.Text.Json.Serialization;

namespace StockApp.Application.DTO
{
    public class StockSearchResponse
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("result")]
        public List<StockSearchResult>? Result { get; set; }
    }

    public class StockSearchResult
    {
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("displaySymbol")]
        public string? DisplaySymbol { get; set; }

        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}
