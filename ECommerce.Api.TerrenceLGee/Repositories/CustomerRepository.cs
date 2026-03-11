using ECommerce.Api.TerrenceLGee.Data;
using ECommerce.Api.TerrenceLGee.Repositories.Helpers;
using ECommerce.Contracts.TerrenceLGee.Common.Extensions;
using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Contracts.TerrenceLGee.Interfaces.RepositoryInterfaces;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.Parameters.CustomerParameters;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.TerrenceLGee.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ECommerceDbContext _context;
    private readonly ILogger<CustomerRepository> _logger;
    private string _errorMessage = string.Empty;

    public CustomerRepository(ECommerceDbContext context, ILogger<CustomerRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ApplicationUser?> GetCustomerProfileAsync(string? customerId)
    {
        try
        {
            var customer = await _context.Users
                .Include(c => c.Addresses)
                .Include(c => c.Sales)
                .ThenInclude(s => s.SaleProducts)
                .FirstOrDefaultAsync(c => c.Id.Equals(customerId));

            return customer;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CustomerRepository)}\n" +
                $"Method: {nameof(GetCustomerProfileAsync)}\n" +
                $"There was an unexpected error retrieving customer {customerId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public async Task<PagedList<ApplicationUser>> GetAllCustomersForAdminAsync(CustomerQueryParams customerQueryParams)
    {
        try
        {
            var count = await GetCountOfAllCustomersForAdminAsync();

            if (count == 0) return [];

            var customers = _context.Users
                .Include(c => c.Addresses)
                .Include(c => c.Sales)
                .ThenInclude(s => s.SaleProducts)
                .AsNoTracking();

            SetFilteringAndSorting(ref customers, customerQueryParams);

            return await customers.ToPagedListAsync(count, customerQueryParams.Page, customerQueryParams.PageSize);
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CustomerRepository)}\n" +
                $"Method: {nameof(GetAllCustomersForAdminAsync)}\n" +
                $"There was an unexpected error retrieving all customers: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
    }

    public async Task<int> GetCountOfAllCustomersForAdminAsync()
    {
        try
        {
            return await _context.Users
                .AsNoTracking()
                .CountAsync();
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CustomerRepository)}\n" +
                $"Method: {nameof(GetCountOfAllCustomersForAdminAsync)}\n" +
                $"There was an unexpected error retrieving the count of all customers: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return -1;
        }
    }

    private void SetFilteringAndSorting(ref IQueryable<ApplicationUser> customers, CustomerQueryParams customerQueryParams)
    {
        if (customerQueryParams.MinSaleCount.HasValue && customerQueryParams.MaxSaleCount.HasValue)
        {
            if (customerQueryParams.IsValidSaleCountRange)
            {
                customers = customers.Where(c => c.Sales.Count >= customerQueryParams.MinSaleCount.Value &&
                c.Sales.Count <= customerQueryParams.MaxSaleCount.Value);
            }
        }
        else if (customerQueryParams.MinSaleCount.HasValue && !customerQueryParams.MaxSaleCount.HasValue)
        {
            customers = customers.Where(c => c.Sales.Count >= customerQueryParams.MinSaleCount);
        }
        else if (customerQueryParams.MaxSaleCount.HasValue && !customerQueryParams.MinSaleCount.HasValue)
        {
            customers = customers.Where(c => c.Sales.Count <= customerQueryParams.MaxSaleCount.Value);
        }

        if (customerQueryParams.MinTotalSpent.HasValue && customerQueryParams.MaxTotalSpent.HasValue)
        {
            if (customerQueryParams.IsValidTotalSpentRange)
            {
                customers = customers.Where(c => c.Sales.Sum(s => s.TotalAmount) >= customerQueryParams.MinTotalSpent.Value &&
                c.Sales.Sum(s => s.TotalAmount) <= customerQueryParams.MaxTotalSpent.Value);
            }
        }
        else if (customerQueryParams.MinTotalSpent.HasValue && !customerQueryParams.MaxTotalSpent.HasValue)
        {
            customers = customers.Where(c => c.Sales.Sum(s => s.TotalAmount) >= customerQueryParams.MinTotalSpent.Value);
        }
        else if (customerQueryParams.MaxTotalSpent.HasValue && !customerQueryParams.MinTotalSpent.HasValue)
        {
            customers = customers.Where(c => c.Sales.Sum(s => s.TotalAmount) <= customerQueryParams.MaxTotalSpent.Value);
        }

        customers = SortHelper<ApplicationUser>.ApplySorting(customers, customerQueryParams.OrderBy);
    }
}
