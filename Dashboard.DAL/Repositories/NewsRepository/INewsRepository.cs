using Dashboard.DAL.Models.Identity.NewsCategory;

namespace Dashboard.DAL.Repositories.NewsRepository
{
    public interface INewsRepository
    {
        Task<News> CreateAsync(News news);
        Task DeleteAsync(int id);
        Task<IEnumerable<News>> GetAllAsync();
        Task<IEnumerable<News>> GetByCategoryIdAsync(int categoryId);
        Task<News> GetByIdAsync(int id);
        Task<News> UpdateAsync(News news);
    }
}