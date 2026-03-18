using ECommerce.AvaloniaClient.TerrenceLGee.Data;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using ECommerce.Shared.TerrenceLGee.DTOs.ProductDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.ProductParameters;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<ProductService> _logger;
    private readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
    private string _errorMessage = string.Empty;
    private const string ClientName = "client";
    private const string MediaType = "application/json";
    private const string LogErrorString = "{msg}\n\n";

    public ProductService(IHttpClientFactory clientFactory, ILogger<ProductService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<ProductAdminData?> AddProductAsync(CreateProductDto product)
    {
        var productAdminDataForError = new ProductAdminData();

        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.AdminAddProductUrl}";

            var content = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, MediaType);

            var response = await httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                productAdminDataForError.ErrorMessage = $"Unable to add new product in category {product.CategoryId}\n" +
                    $"Reason: {response.ReasonPhrase}";
                return productAdminDataForError;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var productAddedResponse = JsonSerializer.Deserialize<ProductAdminRoot>(responseContent, options);

            if (productAddedResponse is null)
            {
                productAdminDataForError.ErrorMessage = $"Unable to add new product in category {product.CategoryId} at this time, " +
                    $"please try again later";
                return productAdminDataForError;
            }

            if (!productAddedResponse.IsSuccess || productAddedResponse.StatusCode != 201)
            {
                productAdminDataForError.ErrorMessage = $"Unable to add new product in category {product.CategoryId}: " +
                    $"{string.Join('\n', productAddedResponse.Errors)}";
                return productAdminDataForError;
            }

            return productAddedResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(AddProductAsync)}\n" +
                $"There was an API error adding a new product in category {product.CategoryId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            productAdminDataForError.ErrorMessage = $"There was an API error adding a new product in category {product.CategoryId}";
            return productAdminDataForError;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(AddProductAsync)}\n" +
                $"There was an unexpected error adding a new product in category {product.CategoryId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            productAdminDataForError.ErrorMessage = $"There was an unexpected error adding a new product in category {product.CategoryId}";
            return productAdminDataForError;
        }
    }

    public async Task<ProductAdminData?> UpdateProductAsync(UpdateProductDto product)
    {
        var productAdminDataForError = new ProductAdminData();

        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.AdminUpdateProductUrl}{product.Id}";

            var content = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                productAdminDataForError.ErrorMessage = $"Unable to update product {product.Id} in category " +
                    $"{product.CategoryId}\nReason: {response.ReasonPhrase}";
                return productAdminDataForError;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var productUpdatedResponse = JsonSerializer.Deserialize<ProductAdminRoot>(responseContent, options);

            if (productUpdatedResponse is null)
            {
                productAdminDataForError.ErrorMessage = $"Unable to update product {product.Id} in category " +
                    $"{product.CategoryId} at this time, please try again later";
                return productAdminDataForError;
            }

            if (!productUpdatedResponse.IsSuccess || productUpdatedResponse.StatusCode != 200)
            {
                productAdminDataForError.ErrorMessage = $"Unable to update product {product.Id} in category " +
                    $"{product.CategoryId}: {string.Join('\n', productUpdatedResponse.Errors)}";
                return productAdminDataForError;
            }

            return productUpdatedResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(UpdateProductAsync)}\n" +
                $"There was an API error updating product {product.Id} in category {product.CategoryId}: " +
                $"{ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            productAdminDataForError.ErrorMessage = $"There was an API error updating product {product.Id} in category {product.CategoryId}";
            return productAdminDataForError;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(UpdateProductAsync)}\n" +
                $"There was an unexpected error updating product {product.Id} in category {product.CategoryId}: " +
                $"{ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            productAdminDataForError.ErrorMessage = $"There was an unexpected error updating product {product.Id} in category {product.CategoryId}";
            return productAdminDataForError;
        }
    }

    public Task<string?> DeleteProductAsync(int productId)
    {
        throw new NotImplementedException();
    }

    public Task<string?> RestoreProductAsync(int productId)
    {
        throw new NotImplementedException();
    }

    public Task<ProductAdminData?> GetProductForAdminAsync(int productId)
    {
        throw new NotImplementedException();
    }

    public Task<ProductData?> GetProductAsync(int productId)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductsAdminRoot?> GetProductsForAdminAsync(ProductQueryParams queryParams)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.AdminGetProductsUrl}{BuildQueryString(queryParams)}";

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            var productsResponse = JsonSerializer.Deserialize<ProductsAdminRoot>(responseContent, options);

            if (productsResponse is null) return null;

            return productsResponse;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(GetProductsForAdminAsync)}\n" +
                $"There was an API error attempting to retrieve all of the products: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(GetProductsForAdminAsync)}\n" +
                $"There was an unexpected error attempting to retrieve all of the products: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
    }

    public Task<ProductRoot?> GetProductsAsync(ProductQueryParams queryParams)
    {
        throw new NotImplementedException();
    }

    private static string BuildQueryString(ProductQueryParams queryParams)
    {
        var query = new StringBuilder();
        query.Append($"?page={queryParams.Page}&pageSize={queryParams.PageSize}");

        if (queryParams.MinUnitPrice.HasValue)
        {
            query.Append($"&minUnitPrice={queryParams.MinUnitPrice}");
        }

        if (queryParams.MaxUnitPrice.HasValue)
        {
            query.Append($"&maxUnitPrice={queryParams.MaxUnitPrice}");
        }

        if (queryParams.MinStockQuantity.HasValue)
        {
            query.Append($"&minStockQuantity={queryParams.MinStockQuantity}");
        }

        if (queryParams.MaxStockQuantity.HasValue)
        {
            query.Append($"&maxStockQuantity={queryParams.MaxStockQuantity}");
        }

        if (queryParams.MinDiscountPercentage.HasValue)
        {
            query.Append($"&minDiscountPercentage={queryParams.MinDiscountPercentage}");
        }

        if (queryParams.MaxDiscountPercentage.HasValue)
        {
            query.Append($"&maxDiscountPercentage={queryParams.MaxDiscountPercentage}");
        }

        if (queryParams.CategoryId.HasValue)
        {
            query.Append($"&categoryId={queryParams.CategoryId}");
        }

        if (!string.IsNullOrEmpty(queryParams.CategoryName))
        {
            query.Append($"&categoryName={queryParams.CategoryName}");
        }

        if (!string.IsNullOrEmpty(queryParams.Description))
        {
            query.Append($"&description={queryParams.Description}");
        }

        if (queryParams.InStock.HasValue)
        {
            query.Append($"&inStock={queryParams.InStock}");
        }

        return query.ToString();
    }
}
