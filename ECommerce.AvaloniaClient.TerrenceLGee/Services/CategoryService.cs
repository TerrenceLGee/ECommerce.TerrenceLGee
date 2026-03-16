using ECommerce.AvaloniaClient.TerrenceLGee.Data;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Category;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Category;
using ECommerce.Shared.TerrenceLGee.DTOs.CategoryDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.CategoryParameters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services;

public class CategoryService : ICategoryService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<CategoryService> _logger;
    private readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
    private string _errorMessage = string.Empty;
    private const string ClientName = "client";
    private const string MediaType = "application/json";
    private const string LogErrorString = "{msg}\n\n";

    public CategoryService(IHttpClientFactory clientFactory, ILogger<CategoryService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<CategoryAdminData?> AddCategoryAsync(CreateCategoryDto category)
    {
        var categoryAdminDataForError = new CategoryAdminData();
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.AdminAddCategoryUrl}";

            var content = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, MediaType);

            var response = await httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                categoryAdminDataForError.ErrorMessage = $"Unable to add new category\n Reason: {response.ReasonPhrase}";
                return categoryAdminDataForError;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var categoryAddedResponse = JsonSerializer.Deserialize<CategoryAdminRoot>(responseContent, options);

            if (categoryAddedResponse is null)
            {
                categoryAdminDataForError.ErrorMessage = $"Unable to add new category at this time, please try again later";
                return categoryAdminDataForError;
            }

            if (!categoryAddedResponse.IsSuccess || categoryAddedResponse.StatusCode != 201)
            {
                categoryAdminDataForError.ErrorMessage = $"Unable to add new category: {string.Join('\n', categoryAddedResponse.Errors)}";
                return categoryAdminDataForError;
            }

            return categoryAddedResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(CategoryService)}\n" +
                $"Method: {nameof(AddCategoryAsync)}\n" +
                $"There was an API error attempting to add the new category: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            categoryAdminDataForError.ErrorMessage = "There was an API error attempting to add the new category.";
            return categoryAdminDataForError;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CategoryService)}\n" +
                $"Method: {nameof(AddCategoryAsync)}\n" +
                $"There was an unexpected error attempting to add the new category: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            categoryAdminDataForError.ErrorMessage = "There was an unexpected error attempting to add the new category.";
            return categoryAdminDataForError;
        }
    }

    public Task<CategoryAdminData?> UpdateCategoryAsync(UpdateCategoryDto category)
    {
        throw new System.NotImplementedException();
    }

    public Task<CategoryData?> GetCategoryByIdAsync(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<CategoryAdminData?> GetCategoryByIdForAdminAsync(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<CategoryData?> GetCategoryByNameAsync(string name)
    {
        throw new System.NotImplementedException();
    }

    public Task<CategoryAdminData?> GetCategoryByNameForAdminAsync(string name)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<CategorySummaryData>> GetCategories(CategoryQueryParams queryParams)
    {
        throw new System.NotImplementedException();
    }

    public async Task<CategoriesAdminRoot?> GetCategoriesForAdmin(CategoryQueryParams queryParams)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.AdminGetCategoriesUrl}{BuildQueryString(queryParams)}";

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            var categoriesResponse = JsonSerializer.Deserialize<CategoriesAdminRoot>(responseContent, options);

            if (categoriesResponse is null) return null;

            return categoriesResponse;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"Class: {nameof(CategoryService)}\n" +
                $"Method: {nameof(GetCategoriesForAdmin)}\n" +
                $"There was an API error attempting to retrieve all of the categories";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _errorMessage = $"Class: {nameof(CategoryService)}\n" +
                $"Method: {nameof(GetCategoriesForAdmin)}\n" +
                $"There was an unexpected error attempting to retrieve all of the categories";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
    }

    private string BuildQueryString(CategoryQueryParams queryParams)
    {
        var query = new StringBuilder();
        query.Append($"?page={queryParams.Page}&pageSize={queryParams.PageSize}");

        if (!string.IsNullOrEmpty(queryParams.Description))
        {
            query.Append($"&description={queryParams.Description}");
        }

        if (!string.IsNullOrEmpty(queryParams.OrderBy))
        {
            query.Append($"&orderBy={queryParams.OrderBy}");
        }

        return query.ToString();
    }
}
