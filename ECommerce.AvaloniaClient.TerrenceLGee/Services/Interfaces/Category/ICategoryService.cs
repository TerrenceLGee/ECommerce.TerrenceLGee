using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Category;
using ECommerce.Shared.TerrenceLGee.DTOs.CategoryDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.CategoryParameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Category;

public interface ICategoryService
{
    Task<CategoryAdminData?> AddCategoryAsync(CreateCategoryDto category);
    Task<CategoryAdminData?> UpdateCategoryAsync(UpdateCategoryDto category);
    Task<CategoryData?> GetCategoryByIdAsync(int id);
    Task<CategoryAdminData?> GetCategoryByIdForAdminAsync(int id);
    Task<CategoryData?> GetCategoryByNameAsync(string name);
    Task<CategoryAdminData?> GetCategoryByNameForAdminAsync(string name);
    Task<List<CategorySummaryData>> GetCategories(CategoryQueryParams queryParams);
    Task<CategoriesAdminRoot?> GetCategoriesForAdmin(CategoryQueryParams queryParams);
}
