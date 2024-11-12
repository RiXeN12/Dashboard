﻿using Dashboard.DAL.Models.Identity;

namespace Dashboard.BLL.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<Category> CreateAsync(Category category);
        Task DeleteAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task<Category> UpdateAsync(Category category);
    }
}