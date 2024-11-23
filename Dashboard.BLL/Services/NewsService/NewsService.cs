using Dashboard.DAL.Models.Identity.NewsCategory;
using Dashboard.DAL.Repositories.NewsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.BLL.Services.NewsService
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;

        public NewsService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<IEnumerable<News>> GetAllAsync()
        {
            return await _newsRepository.GetAllAsync();
        }

        public async Task<News> GetByIdAsync(int id)
        {
            return await _newsRepository.GetByIdAsync(id);
        }

        public async Task<News> CreateAsync(News news)
        {
            return await _newsRepository.CreateAsync(news);
        }

        public async Task<News> UpdateAsync(News news)
        {
            return await _newsRepository.UpdateAsync(news);
        }

        public async Task DeleteAsync(int id)
        {
            await _newsRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<News>> GetByCategoryIdAsync(int categoryId)
        {
            return await _newsRepository.GetByCategoryIdAsync(categoryId);
        }
    }
}
