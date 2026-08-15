using Microsoft.Extensions.Logging;
using StockApp.Application.DTO;
using StockApp.Domain.Entities;

namespace StockApp.Application.Services
{
    public interface INewsRepository
    {
        Task<List<News>> GetLatestNewsAsync(int count = 20);
        Task<List<News>> GetNewsByCategoryAsync(string category, int count = 20);
        Task<List<News>> GetNewsPublishedAfterAsync(DateTime date);
        Task<List<News>> GetAllAsync();
        Task AddAsync(News news);
        Task DeleteAsync(Guid newsId);
        Task SaveAsync();
    }

    public interface IRssFeedService
    {
        Task<List<News>> FetchNewsFromRssAsync(string rssUrl, string category);
    }

    public interface INewsService
    {
        Task<List<NewsResponse>> GetLatestNewsAsync(int count = 20);
        Task<List<NewsResponse>> GetNewsByCategoryAsync(string category, int count = 20);
        Task<bool> RefreshNewsFromRssAsync();
    }

    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IRssFeedService _rssFeedService;
        private readonly ILogger<NewsService> _logger;

        // Popular financial news RSS feeds
        private readonly List<(string Url, string Category)> _newsFeedSources = new()
        {
            ("https://feeds.bloomberg.com/markets/news.rss", "Financial"),
            ("http://feeds.reuters.com/reuters/businessNews", "Business"),
            ("https://feeds.finance.yahoo.com/rss/2.0/headline", "Markets"),
            ("https://www.cnbc.com/id/100003114/device/rss/rss.html", "Markets"),
            ("https://feeds.bloomberg.com/technology/news.rss", "Technology"),
        };

        public NewsService(
            INewsRepository newsRepository,
            IRssFeedService rssFeedService,
            ILogger<NewsService> logger)
        {
            _newsRepository = newsRepository;
            _rssFeedService = rssFeedService;
            _logger = logger;
        }

        public async Task<List<NewsResponse>> GetLatestNewsAsync(int count = 20)
        {
            var news = await _newsRepository.GetLatestNewsAsync(count);
            return news.Select(n => MapToResponse(n)).ToList();
        }

        public async Task<List<NewsResponse>> GetNewsByCategoryAsync(string category, int count = 20)
        {
            var news = await _newsRepository.GetNewsByCategoryAsync(category, count);
            return news.Select(n => MapToResponse(n)).ToList();
        }

        public async Task<bool> RefreshNewsFromRssAsync()
        {
            try
            {
                _logger.LogInformation("Starting news refresh from RSS feeds");

                var allNewsFeed = new List<News>();

                foreach (var (url, category) in _newsFeedSources)
                {
                    try
                    {
                        var feedNews = await _rssFeedService.FetchNewsFromRssAsync(url, category);
                        allNewsFeed.AddRange(feedNews);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Failed to fetch from {url}: {ex.Message}");
                        // Continue with next feed
                    }
                }

                // Remove old news (older than 7 days)
                var oldNewsDate = DateTime.UtcNow.AddDays(-7);
                var oldNews = await _newsRepository.GetNewsPublishedAfterAsync(oldNewsDate.AddDays(-8));

                foreach (var oldItem in oldNews.Where(n => n.PublishedDate < oldNewsDate))
                {
                    await _newsRepository.DeleteAsync(oldItem.NewsID);
                }

                // Add new news items (avoiding duplicates)
                foreach (var news in allNewsFeed)
                {
                    // Check if news with same headline already exists
                    var existingNews = await _newsRepository.GetAllAsync();
                    if (!existingNews.Any(n => n.Headline.Equals(news.Headline, StringComparison.OrdinalIgnoreCase)))
                    {
                        await _newsRepository.AddAsync(news);
                    }
                }

                await _newsRepository.SaveAsync();
                _logger.LogInformation($"Successfully refreshed news. Added {allNewsFeed.Count} new items");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error refreshing news: {ex.Message}");
                return false;
            }
        }

        private NewsResponse MapToResponse(News news)
        {
            return new NewsResponse
            {
                ID = news.NewsID.GetHashCode(),
                Headline = news.Headline,
                Text = news.Text,
                Image = news.Image ?? "",
                PublishedDate = news.PublishedDate,
                Category = news.Category,
                Source = news.Source
            };
        }
    }
}
