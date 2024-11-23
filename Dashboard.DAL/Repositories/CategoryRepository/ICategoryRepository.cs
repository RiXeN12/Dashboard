using Dashboard.DAL.Models.Identity.NewsCategory;

namespace Dashboard.DAL.Repositories.CategoryRepository
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);
        Task DeleteAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task<Category> GetByIdWithNewsAsync(int id);
        Task<Category> UpdateAsync(Category category);
    }
}