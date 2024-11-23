using Dashboard.DAL.Data;
using Dashboard.DAL.Models.Identity.NewsCategory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.DAL.Repositories.NewsRepository
{
    public class NewsRepository : INewsRepository
    {
        private readonly AppDbContext _context;

        public NewsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<News>> GetAllAsync()
        {
            return await _context.News
                .Include(n => n.Category)
                .ToListAsync();
        }

        public async Task<News> GetByIdAsync(int id)
        {
            return await _context.News
                .Include(n => n.Category)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<News> CreateAsync(News news)
        {
            news.CreatedAt = DateTime.UtcNow;
            _context.News.Add(news);
            await _context.SaveChangesAsync();
            return news;
        }

        public async Task<News> UpdateAsync(News news)
        {
            news.UpdatedAt = DateTime.UtcNow;
            _context.News.Update(news);
            await _context.SaveChangesAsync();
            return news;
        }

        public async Task DeleteAsync(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                _context.News.Remove(news);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<News>> GetByCategoryIdAsync(int categoryId)
        {
            return await _context.News
                .Where(n => n.CategoryId == categoryId)
                .Include(n => n.Category)
                .ToListAsync();
        }
    }
}
