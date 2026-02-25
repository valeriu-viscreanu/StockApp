using System.Text.Json.Serialization;

namespace StockApp.Application.DTO
{
    public class FinnhubSearchResponse
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("result")]
        public List<FinnhubSearchResult>? Result { get; set; }
    }

    public class FinnhubSearchResult
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
