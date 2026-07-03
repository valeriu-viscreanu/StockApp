using System.Xml;
using Microsoft.Extensions.Logging;
using StockApp.Application.Services;
using StockApp.Domain.Entities;

namespace StockApp.Infrastructure.Services
{
    public class RssFeedService : IRssFeedService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RssFeedService> _logger;

        public RssFeedService(HttpClient httpClient, ILogger<RssFeedService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<News>> FetchNewsFromRssAsync(string rssUrl, string category)
        {
            var newsList = new List<News>();

            try
            {
                _logger.LogInformation($"Fetching news from RSS feed: {rssUrl}");

                var response = await _httpClient.GetAsync(rssUrl);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to fetch RSS feed. Status: {response.StatusCode}");
                    return newsList;
                }

                var content = await response.Content.ReadAsStringAsync();
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(content);

                var xmlNamespace = new XmlNamespaceManager(new NameTable());
                xmlNamespace.AddNamespace("content", "http://purl.org/rss/1.0/modules/content/");
                xmlNamespace.AddNamespace("atom", "http://www.w3.org/2005/Atom");

                var items = xmlDoc.GetElementsByTagName("item");

                foreach (XmlElement item in items)
                {
                    try
                    {
                        var titleNode = item.GetElementsByTagName("title")[0];
                        var descriptionNode = item.GetElementsByTagName("description")[0];
                        var pubDateNode = item.GetElementsByTagName("pubDate")[0];
                        var linkNode = item.GetElementsByTagName("link")[0];

                        var title = titleNode?.InnerText ?? "No Title";
                        var description = descriptionNode?.InnerText ?? "No Description";
                        var pubDateStr = pubDateNode?.InnerText ?? DateTime.UtcNow.ToString();
                        var link = linkNode?.InnerText ?? "";

                        // Parse publish date
                        var publishedDate = ParseRssDate(pubDateStr);

                        var news = new News
                        {
                            NewsID = Guid.NewGuid(),
                            Headline = TruncateString(title, 500),
                            Text = description,
                            Source = ExtractSourceFromUrl(rssUrl),
                            SourceUrl = link,
                            PublishedDate = publishedDate,
                            CreatedDate = DateTime.UtcNow,
                            Category = category,
                            Image = null // You can add image extraction logic here if needed
                        };

                        newsList.Add(news);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Error parsing RSS item: {ex.Message}");
                        continue;
                    }
                }

                _logger.LogInformation($"Successfully fetched {newsList.Count} news items from {rssUrl}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching RSS feed from {rssUrl}: {ex.Message}");
            }

            return newsList;
        }

        private DateTime ParseRssDate(string dateStr)
        {
            // Try standard RSS date formats
            if (DateTime.TryParse(dateStr, out var result))
            {
                return result;
            }

            // Return current UTC time if parsing fails
            return DateTime.UtcNow;
        }

        private string ExtractSourceFromUrl(string url)
        {
            try
            {
                var uri = new Uri(url);
                return uri.Host;
            }
            catch
            {
                return "Unknown";
            }
        }

        private string TruncateString(string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input) || input.Length <= maxLength)
                return input;

            return input.Substring(0, maxLength - 3) + "...";
        }
    }
}
