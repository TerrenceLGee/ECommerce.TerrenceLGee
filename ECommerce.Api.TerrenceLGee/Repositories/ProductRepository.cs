using ECommerce.Api.TerrenceLGee.Data;
using ECommerce.Api.TerrenceLGee.Repositories.Helpers;
using ECommerce.Contracts.TerrenceLGee.Common.Extensions;
using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Contracts.TerrenceLGee.Interfaces.RepositoryInterfaces;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.Parameters.ProductParameters;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.TerrenceLGee.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ECommerceDbContext _context;
    private readonly ILogger<ProductRepository> _logger;
    private string _errorMessage = string.Empty;

    public ProductRepository(ECommerceDbContext context, ILogger<ProductRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Product?> AddProductAsync(Product product)
    {
        try
        {
            var productAlreadyExists = await _context.Products
                .AnyAsync(p => p.Name.ToLower().Equals(product.Name.ToLower()));

            if (productAlreadyExists) return null;

            product.CreatedAt = DateTime.UtcNow;

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductRepository)}\n" +
                $"Method: {nameof(AddProductAsync)}\n" +
                $"There was an unexpected error adding a new product to category {product.CategoryId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }

    }

    public async Task<Product?> UpdateProductAsync(Product product)
    {
        try
        {
            var productToUpdate = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (productToUpdate is null) return null;

            productToUpdate.Name = product.Name;
            productToUpdate.Description = product.Description;
            productToUpdate.StockQuantity = product.StockQuantity;
            productToUpdate.DiscountPercentage = product.DiscountPercentage;

            if (productToUpdate.StockQuantity <= 0)
            {
                productToUpdate.IsInStock = false;
            }

            productToUpdate.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return productToUpdate;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductRepository)}\n" +
                $"Method: {nameof(UpdateProductAsync)}\n" +
                $"There was an unexpected error updating product {product.Id} in category {product.CategoryId}: " +
                $"{ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public async Task<bool> DeleteProductAsync(ProductParams productParams)
    {
        try
        {
            var productToDelete = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productParams.ProductId);

            if (productToDelete is null || productToDelete.IsDeleted) return false;

            productToDelete.IsDeleted = true;
            productToDelete.IsInStock = false;
            productToDelete.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductRepository)}\n" +
                $"Method: {nameof(DeleteProductAsync)}\n" +
                $"There was an unexpected error 'deleting' product: {productParams.ProductId} from category " +
                $"{productParams.CategoryId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return false;
        }
    }

    public async Task<bool> RestoreProductAsync(ProductParams productParams)
    {
        try
        {
            var productToRestore = await _context.Products
                 .FirstOrDefaultAsync(p => p.Id == productParams.ProductId && p.IsDeleted);

            if (productToRestore is null || !productToRestore.IsDeleted) return false;

            productToRestore.IsDeleted = false;
            if (productToRestore.StockQuantity > 0) productToRestore.IsInStock = true;
            productToRestore.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductRepository)}\n" +
            $"Method: {nameof(RestoreProductAsync)}\n" +
            $"There was an unexpected error 'restoring' product: {productParams.ProductId} into category " +
            $"{productParams.CategoryId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return false;
        }
    }

    public async Task<Product?> GetProductAsync(ProductParams productParams)
    {
        try
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == productParams.ProductId);

            return product;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductRepository)}\n" +
                $"Method: {nameof(GetProductAsync)}\n" +
                $"There was an unexpected error retrieving product {productParams.ProductId} from category " +
                $"{productParams.CategoryId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public async Task<Product?> GetProductByNameAsync(ProductParams productParams)
    {
        try
        {
            if (string.IsNullOrEmpty(productParams.ProductName)) return null;

            var product = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Name.ToLower().Equals(productParams.ProductName.ToLower()));

            return product;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductRepository)}\n" +
                $"Method: {nameof(GetProductByNameAsync)}\n" +
                $"There was an unexpected error retrieving product {productParams.ProductName} in category " +
                $"{productParams.CategoryId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public async Task<PagedList<Product>> GetProductsAsync(ProductQueryParams productQueryParams)
    {
        try
        {
            var count = await GetProductCountAsync();

            if (count == 0) return [];

            var products = _context.Products
                .Include(p => p.Category)
                .AsNoTracking();

            SetFilteringAndSorting(ref products, productQueryParams);

            return await products.ToPagedListAsync(count, productQueryParams.Page, productQueryParams.PageSize);
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductRepository)}\n" +
                $"Method: {nameof(GetProductsAsync)}\n" +
                $"There was an unexpected error retrieving all products: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
    }


    public async Task<int> GetProductCountInCategoryAsync(ProductParams productParams)
    {
        try
        {
            var count = 0;

            if (!string.IsNullOrEmpty(productParams.CategoryName))
            {
                count = await _context.Products
                    .AsNoTracking()
                    .CountAsync(p => p.Category != null && p.Category.Name.ToLower().Equals(productParams.CategoryName.ToLower()));
            }
            else
            {
                count = await _context.Products
                    .AsNoTracking()
                    .CountAsync(p => p.Category != null && p.CategoryId == productParams.CategoryId);
            }

            return count;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductRepository)}\n" +
                $"Method: {nameof(GetProductCountInCategoryAsync)}\n" +
                $"There was an unexpected error retrieving the count of the products in category " +
                $"{productParams.CategoryId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return -1;
        }
    }

    public async Task<int> GetProductCountAsync()
    {
        try
        {
            return await _context.Products
                .AsNoTracking()
                .CountAsync();
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductRepository)}\n" +
                $"Method: {nameof(GetProductCountAsync)}\n" +
                $"There was an unexpected error retrieving count of products: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return -1;
        }
    }

    private void SetFilteringAndSorting(ref IQueryable<Product> products, ProductQueryParams productQueryParams)
    {
        if (!string.IsNullOrEmpty(productQueryParams.CategoryName))
        {
            products = products.Where(p => p.Category != null &&
            p.Category.Name.ToLower().Equals(productQueryParams.CategoryName.ToLower()));
        }

        if (!string.IsNullOrEmpty(productQueryParams.Description))
        {
            products = products.Where(p => !string.IsNullOrEmpty(p.Description) &&
            p.Description.ToLower().Contains(productQueryParams.Description.ToLower()));
        }

        if (productQueryParams.MinUnitPrice.HasValue && productQueryParams.MaxUnitPrice.HasValue)
        {
            if (productQueryParams.IsValidUnitPriceRange)
            {
                products = products.Where(p => p.UnitPrice >= productQueryParams.MinUnitPrice &&
                p.UnitPrice <= productQueryParams.MaxUnitPrice);
            }
        }
        else if (productQueryParams.MinUnitPrice.HasValue && !productQueryParams.MaxUnitPrice.HasValue)
        {
            products = products.Where(p => p.UnitPrice >= productQueryParams.MinUnitPrice);
        }
        else if (productQueryParams.MaxUnitPrice.HasValue && !productQueryParams.MinUnitPrice.HasValue)
        {
            products = products.Where(p => p.UnitPrice <= productQueryParams.MaxUnitPrice);
        }

        if (productQueryParams.MinStockQuantity.HasValue && productQueryParams.MaxStockQuantity.HasValue)
        {
            if (productQueryParams.IsValidStockQuantityRange)
            {
                products = products.Where(p => p.StockQuantity >= productQueryParams.MinStockQuantity &&
                p.StockQuantity <= productQueryParams.MaxStockQuantity);
            }
        }
        else if (productQueryParams.MinStockQuantity.HasValue && !productQueryParams.MaxStockQuantity.HasValue)
        {
            products = products.Where(p => p.StockQuantity >= productQueryParams.MinStockQuantity);
        }
        else if (productQueryParams.MaxStockQuantity.HasValue && !productQueryParams.MinStockQuantity.HasValue)
        {
            products = products.Where(p => p.StockQuantity <= productQueryParams.MaxStockQuantity);
        }

        if (productQueryParams.MinDiscountPercentage.HasValue && productQueryParams.MaxDiscountPercentage.HasValue)
        {
            if (productQueryParams.IsValidDiscountPercentageRange)
            {
                products = products.Where(p => p.DiscountPercentage >= productQueryParams.MinDiscountPercentage &&
                p.DiscountPercentage <= productQueryParams.MaxDiscountPercentage);
            }
        }
        else if (productQueryParams.MinDiscountPercentage.HasValue && !productQueryParams.MaxDiscountPercentage.HasValue)
        {
            products = products.Where(p => p.DiscountPercentage >= productQueryParams.MinDiscountPercentage);
        }
        else if (productQueryParams.MaxDiscountPercentage.HasValue && !productQueryParams.MinDiscountPercentage.HasValue)
        {
            products = products.Where(p => p.DiscountPercentage <= productQueryParams.MaxDiscountPercentage);
        }

        if (productQueryParams.InStock.HasValue)
        {
            products = products.Where(p => p.IsInStock == productQueryParams.InStock);
        }

        products = SortHelper<Product>.ApplySorting(products, productQueryParams.OrderBy);
    }


}
