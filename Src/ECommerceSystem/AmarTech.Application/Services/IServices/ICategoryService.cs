﻿using AmarTech.Domain.Entities;

namespace AmarTech.Application.Services.IServices
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories();
        Category? GetCategoryById(int? id);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int? id);
    }
}
