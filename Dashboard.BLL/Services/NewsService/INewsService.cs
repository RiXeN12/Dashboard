using Dashboard.DAL.Models.Identity.NewsCategory;

namespace Dashboard.BLL.Services.NewsService
{
    public interface INewsService
    {
        Task<News> CreateAsync(News news);
        Task DeleteAsync(int id);
        Task<IEnumerable<News>> GetAllAsync();
        Task<IEnumerable<News>> GetByCategoryIdAsync(int categoryId);
        Task<News> GetByIdAsync(int id);
        Task<News> UpdateAsync(News news);
    }
}