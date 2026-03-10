using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.Parameters.ProductParameters;

namespace ECommerce.Contracts.TerrenceLGee.Interfaces.RepositoryInterfaces;

public interface IProductRepository
{
    Task<Product?> AddProductAsync(Product product);
    Task<Product?> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(ProductParams productParams);
    Task<bool> RestoreProductAsync(ProductParams productParams);
    Task<Product?> GetProductAsync(ProductParams productParams);
    Task<Product?> GetProductByNameAsync(ProductParams productParams);
    Task<PagedList<Product>> GetProductsAsync(ProductQueryParams productQueryParams);
    Task<int> GetProductCountInCategoryAsync(ProductParams productParams);
    Task<int> GetProductCountAsync();
}
