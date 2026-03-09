using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Contracts.TerrenceLGee.Common.Results;
using ECommerce.Contracts.TerrenceLGee.Interfaces.RepositoryInterfaces;
using ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;
using ECommerce.Contracts.TerrenceLGee.Mappings.CategoryMappings;
using ECommerce.Shared.TerrenceLGee.DTOs.CategoryDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.CategoryParameters;

namespace ECommerce.Api.TerrenceLGee.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<RetrievedCategoryDto?>> AddCategoryAsync(CreateCategoryDto category)
    {
        var addedCategory = await _categoryRepository.AddCategoryAsync(category.FromCreateCategoryDto());

        if (addedCategory is null)
        {
            return Result<RetrievedCategoryDto?>.Fail("Unable to add new category", ErrorType.BadRequest);
        }

        return Result<RetrievedCategoryDto?>.Ok(addedCategory.ToRetrievedCategoryDto());
    }

    public async Task<Result<RetrievedCategoryDto?>> UpdateCategoryAsync(UpdateCategoryDto category)
    {
        var updatedCategory = await _categoryRepository.UpdateCategoryAsync(category.FromUpdateCategoryDto());

        if (updatedCategory is null)
        {
            return Result<RetrievedCategoryDto?>.Fail($"Unable to update category {category.Id}", ErrorType.BadRequest);
        }

        return Result<RetrievedCategoryDto?>.Ok(updatedCategory.ToRetrievedCategoryDto());
    }

    public async Task<Result<RetrievedCategoryDto?>> GetCategoryAsync(CategoryParams categoryParams)
    {
        var category = await _categoryRepository.GetCategoryAsync(categoryParams);

        if (category is null)
        {
            return Result<RetrievedCategoryDto?>.Fail($"Unable to retrieve category {categoryParams.CategoryId}", ErrorType.NotFound);
        }

        return Result<RetrievedCategoryDto?>.Ok(category.ToRetrievedCategoryDto());
    }

    public async Task<Result<RetrievedCategoryDtoForAdmin?>> GetCategoryForAdminAsync(CategoryParams categoryParams)
    {
        var category = await _categoryRepository.GetCategoryAsync(categoryParams);

        if (category is null)
        {
            return Result<RetrievedCategoryDtoForAdmin?>.Fail($"Unable to retrieve category {categoryParams.CategoryId}", ErrorType.NotFound);
        }

        return Result<RetrievedCategoryDtoForAdmin?>.Ok(category.ToRetrievedCategoryDtoForAdmin());
    }

    public async Task<Result<PagedList<RetrievedCategorySummaryDto>>> GetCategoriesAsync(CategoryQueryParams categoryQueryParams)
    {
        var categories = await _categoryRepository.GetCategoriesAsync(categoryQueryParams);

        var pagedCategories = new PagedList<RetrievedCategorySummaryDto>(
            categories.Select(c => c.ToRetrievedCategorySummaryDto()),
            categories.TotalEntities,
            categoryQueryParams.Page,
            categoryQueryParams.PageSize);

        return Result<PagedList<RetrievedCategorySummaryDto>>.Ok(pagedCategories);
    }

    public async Task<Result<PagedList<RetrievedCategorySummaryForAdminDto>>> GetCategoriesForAdminAsync(CategoryQueryParams categoryQueryParams)
    {
        var categories = await _categoryRepository.GetCategoriesAsync(categoryQueryParams);

        var pagedCategories = new PagedList<RetrievedCategorySummaryForAdminDto>(
            categories.Select(c => c.ToRetrievedCategorySummaryDtoForAdmin()),
            categories.TotalEntities,
            categoryQueryParams.Page,
            categoryQueryParams.PageSize);

        return Result<PagedList<RetrievedCategorySummaryForAdminDto>>.Ok(pagedCategories);
    }

    public async Task<Result<int>> GetCategoriesCountAsync()
    {
        var count = await _categoryRepository.GetCategoriesCountAsync();

        if (count == -1)
        {
            return Result<int>.Fail("Unable to retrieve count of categories", ErrorType.NotFound);
        }

        return Result<int>.Ok(count);
    }


}
