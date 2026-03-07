using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Contracts.TerrenceLGee.Common.Results;
using ECommerce.Shared.TerrenceLGee.DTOs.ProductDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.ProductParameters;

namespace ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;

public interface IProductService
{
    Task<Result<RetrievedProductDto?>> AddProductAsync(CreateProductDto product);
    Task<Result<RetrievedProductDto?>> UpdateProductAsync(UpdateProductDto product);
    Task<Result> DeleteProductAsync(ProductParams productParams);
    Task<Result> RestoreProductAsync(ProductParams productParams);
    Task<Result<RetrievedProductDto?>> GetProductAsync(ProductParams productParams);
    Task<Result<RetrievedProductDtoForAdmin>> GetProductForAdminAsync(ProductParams productParams);
    Task<Result<PagedList<RetrievedProductDto>>> GetProductsAsync(ProductQueryParams productQueryParams);
    Task<Result<int>> GetProductCountInCategoryAsync(ProductParams productParams);
    Task<Result<int>> GetProductCountAsync();
}