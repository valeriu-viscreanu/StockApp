using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using System.Collections.Concurrent;

namespace StockApp.Infrastructure.Services
{

    //TODO decouple this further
    public class MarketDataService : IMarketDataService
    {
        private readonly HttpClient _httpClient;
        private static readonly ConcurrentDictionary<string, (StockQuoteResponse Quote, DateTime Expiry)> _quoteCache = new();
        private const string YahooBaseUrl = "https://query1.finance.yahoo.com";

        public MarketDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private HttpRequestMessage CreateYahooRequest(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
            return request;
        }

        public async Task<CompanyProfileResponse?> GetCompanyProfile(string stockSymbol)
        {
            try
            {
                var request = CreateYahooRequest($"{YahooBaseUrl}/v8/finance/chart/{stockSymbol}?interval=1d&range=1d");
                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode) return GetMockProfile(stockSymbol);

                var yahoo = await response.Content.ReadFromJsonAsync<YahooChartRoot>();
                var meta = yahoo?.Chart?.Result?.FirstOrDefault()?.Meta;
                if (meta == null) return GetMockProfile(stockSymbol);

                return new CompanyProfileResponse
                {
                    Name = meta.ShortName ?? $"{stockSymbol} Inc.",
                    Ticker = stockSymbol,
                    Logo = $"https://logo.clearbit.com/{stockSymbol.ToLower()}.com"
                };
            }
            catch
            {
                return GetMockProfile(stockSymbol);
            }
        }

        private CompanyProfileResponse GetMockProfile(string symbol) => new()
        {
            Name = $"{symbol} Inc.",
            Ticker = symbol,
            Logo = $"https://logo.clearbit.com/{symbol.ToLower()}.com"
        };

        public async Task<StockQuoteResponse?> GetStockPriceQuote(string stockSymbol)
        {
            if (_quoteCache.TryGetValue(stockSymbol, out var cached) && cached.Expiry > DateTime.UtcNow)
            {
                return cached.Quote;
            }

            StockQuoteResponse? result = null;

            try
            {
                var request = CreateYahooRequest($"{YahooBaseUrl}/v8/finance/chart/{stockSymbol}?interval=1d&range=2d");
                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode) return null;

                var yahoo = await response.Content.ReadFromJsonAsync<YahooChartRoot>();
                var meta = yahoo?.Chart?.Result?.FirstOrDefault()?.Meta;
                if (meta == null) return null;

                double currentPrice = meta.RegularMarketPrice ?? 0;
                double previousClose = meta.PreviousClose ?? currentPrice;
                double change = currentPrice - previousClose;
                double percentChange = previousClose != 0 ? (change / previousClose) * 100 : 0;

                result = new StockQuoteResponse
                {
                    CurrentPrice = currentPrice,
                    Change = Math.Round(change, 2),
                    PercentChange = Math.Round(percentChange, 2),
                    HighPriceDay = meta.RegularMarketDayHigh,
                    LowPriceDay = meta.RegularMarketDayLow,
                    OpenPriceDay = meta.RegularMarketOpen,
                    PreviousClosePrice = previousClose
                };
            }
            catch
            {
                return null;
            }

            if (result != null)
            {
                _quoteCache[stockSymbol] = (result, DateTime.UtcNow.AddSeconds(60));
            }

            return result;
        }

        public async Task<StockSearchResponse?> SearchStocks(string query)
        {
            try
            {
                var request = CreateYahooRequest($"{YahooBaseUrl}/v1/finance/search?q={query}&quotesCount=10&newsCount=0");
                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode) return null;

                var yahoo = await response.Content.ReadFromJsonAsync<YahooSearchRoot>();
                if (yahoo?.Quotes == null) return null;

                return new StockSearchResponse
                {
                    Count = yahoo.Quotes.Count,
                    Result = yahoo.Quotes.Select(q => new StockSearchResult
                    {
                        Description = q.ShortName ?? q.LongName ?? "",
                        DisplaySymbol = q.Symbol ?? "",
                        Symbol = q.Symbol ?? "",
                        Type = q.TypeDisp ?? "Equity"
                    }).ToList()
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<StockDataResponse?> GetStockData(string stockSymbol, string resolution, long from, long to)
        {
            string interval;
            string range;
            long span = to - from;

            if (resolution == "60" || span < 86400 * 2) 
            {
                interval = "5m";
                range = "1d";
            }
            else if (span < 86400 * 90) 
            {
                interval = "1d";
                range = "1mo";
            }
            else 
            {
                interval = "1wk";
                range = "1y";
            }

            try
            {
                var request = CreateYahooRequest($"{YahooBaseUrl}/v8/finance/chart/{stockSymbol}?interval={interval}&range={range}");
                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode) return null;

                var yahoo = await response.Content.ReadFromJsonAsync<YahooChartRoot>();
                var result = yahoo?.Chart?.Result?.FirstOrDefault();
                if (result?.Timestamp == null || result.Indicators?.Quote?.FirstOrDefault() == null)
                    return null;

                var q = result.Indicators.Quote[0];

                return new StockDataResponse
                {
                    Status = "ok",
                    Timestamps = result.Timestamp,
                    ClosePrices = q.Close?.Select(v => v ?? 0).ToList(),
                    OpenPrices = q.Open?.Select(v => v ?? 0).ToList(),
                    HighPrices = q.High?.Select(v => v ?? 0).ToList(),
                    LowPrices = q.Low?.Select(v => v ?? 0).ToList(),
                    Volumes = q.Volume?.Select(v => (double)(v ?? 0)).ToList()
                };
            }
            catch
            {
                return null;
            }
        }

        private class YahooChartRoot
        {
            [JsonPropertyName("chart")]
            public YahooChart? Chart { get; set; }
        }

        private class YahooChart
        {
            [JsonPropertyName("result")]
            public List<YahooChartResult>? Result { get; set; }
        }

        private class YahooChartResult
        {
            [JsonPropertyName("meta")]
            public YahooMeta? Meta { get; set; }

            [JsonPropertyName("timestamp")]
            public List<long>? Timestamp { get; set; }

            [JsonPropertyName("indicators")]
            public YahooIndicators? Indicators { get; set; }
        }

        private class YahooMeta
        {
            [JsonPropertyName("regularMarketPrice")]
            public double? RegularMarketPrice { get; set; }

            [JsonPropertyName("previousClose")]
            public double? PreviousClose { get; set; }

            [JsonPropertyName("regularMarketDayHigh")]
            public double? RegularMarketDayHigh { get; set; }

            [JsonPropertyName("regularMarketDayLow")]
            public double? RegularMarketDayLow { get; set; }

            [JsonPropertyName("regularMarketOpen")]
            public double? RegularMarketOpen { get; set; }

            [JsonPropertyName("shortName")]
            public string? ShortName { get; set; }
        }

        private class YahooIndicators
        {
            [JsonPropertyName("quote")]
            public List<YahooQuoteData>? Quote { get; set; }
        }

        private class YahooQuoteData
        {
            [JsonPropertyName("close")]
            public List<double?>? Close { get; set; }

            [JsonPropertyName("open")]
            public List<double?>? Open { get; set; }

            [JsonPropertyName("high")]
            public List<double?>? High { get; set; }

            [JsonPropertyName("low")]
            public List<double?>? Low { get; set; }

            [JsonPropertyName("volume")]
            public List<long?>? Volume { get; set; }
        }

        private class YahooSearchRoot
        {
            [JsonPropertyName("quotes")]
            public List<YahooSearchQuote>? Quotes { get; set; }
        }

        private class YahooSearchQuote
        {
            [JsonPropertyName("symbol")]
            public string? Symbol { get; set; }

            [JsonPropertyName("shortname")]
            public string? ShortName { get; set; }

            [JsonPropertyName("longname")]
            public string? LongName { get; set; }

            [JsonPropertyName("typeDisp")]
            public string? TypeDisp { get; set; }
        }
    }
}
