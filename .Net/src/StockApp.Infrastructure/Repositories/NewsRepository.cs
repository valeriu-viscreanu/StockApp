using StockApp.Application.Services;
using StockApp.Domain.Entities;
using StockApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace StockApp.Infrastructure.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private readonly ApplicationDbContext _context;

        public NewsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<News>> GetLatestNewsAsync(int count = 20)
        {
            return await _context.NewsItems
                .OrderByDescending(n => n.PublishedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<News>> GetNewsByCategoryAsync(string category, int count = 20)
        {
            return await _context.NewsItems
                .Where(n => n.Category == category)
                .OrderByDescending(n => n.PublishedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<News>> GetNewsPublishedAfterAsync(DateTime date)
        {
            return await _context.NewsItems
                .Where(n => n.PublishedDate > date)
                .OrderByDescending(n => n.PublishedDate)
                .ToListAsync();
        }

        public async Task<List<News>> GetAllAsync()
        {
            return await _context.NewsItems.ToListAsync();
        }

        public async Task AddAsync(News news)
        {
            await _context.NewsItems.AddAsync(news);
        }

        public async Task DeleteAsync(Guid newsId)
        {
            var news = await _context.NewsItems.FindAsync(newsId);
            if (news != null)
            {
                _context.NewsItems.Remove(news);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
