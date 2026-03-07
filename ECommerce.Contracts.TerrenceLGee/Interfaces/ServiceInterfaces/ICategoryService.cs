using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Contracts.TerrenceLGee.Common.Results;
using ECommerce.Shared.TerrenceLGee.DTOs.CategoryDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.CategoryParameters;

namespace ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;

public interface ICategoryService
{
    Task<Result<RetrievedCategoryDto?>> AddCategoryAsync(CreateCategoryDto category);
    Task<Result<RetrievedCategoryDto?>> UpdateCategoryAsync(UpdateCategoryDto category);
    Task<Result<RetrievedCategoryDto?>> GetCategoryAsync(CategoryParams categoryParams);
    Task<Result<RetrievedCategoryDtoForAdmin>> GetCategoryForAdminAsync(CategoryParams categoryParams);
    Task<Result<PagedList<RetrievedCategorySummaryDto>>> GetCategoriesAsync(CategoryQueryParams categoryQueryParams);
    Task<Result<int>> GetCategoriesCountAsync();
}
