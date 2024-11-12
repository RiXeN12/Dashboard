using Dashboard.DAL.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.DAL.Repositories.CategoryRepository
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);
        Task DeleteAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task<Category> UpdateAsync(Category category);
    }
}
