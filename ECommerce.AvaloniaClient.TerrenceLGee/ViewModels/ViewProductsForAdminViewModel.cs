using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.ProductMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using ECommerce.Shared.TerrenceLGee.Parameters.ProductParameters;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class ViewProductsForAdminViewModel : ObservableObject
{
    private readonly IProductService _productService;
    private readonly IMessenger _messenger;
    public ObservableCollection<ProductAdminData> Products { get; } = [];

    [ObservableProperty]
    private bool _isLoading;
    [ObservableProperty]
    private ProductAdminData? _selectedProduct;

    [ObservableProperty]
    private int _page = 1;
    [ObservableProperty]
    private int _pageSize = 10;
    [ObservableProperty]
    private int _totalPages;
    [ObservableProperty]
    private bool _hasPreviousPage;
    [ObservableProperty]
    private bool _hasNextPage;

    [ObservableProperty]
    private decimal? _minUnitPrice;
    [ObservableProperty]
    private decimal? _maxUnitPrice;
    [ObservableProperty]
    private int? _minStockQuantity;
    [ObservableProperty]
    private int? _maxStockQuantity;
    [ObservableProperty]
    private int? _minDiscountPercentage;
    [ObservableProperty]
    private int? _maxDiscountPercentage;
    [ObservableProperty]
    private string? _categoryName;
    [ObservableProperty]
    private string? _description;
    [ObservableProperty]
    private bool? _inStock;
    [ObservableProperty]
    private bool? _isDeleted;

    public ViewProductsForAdminViewModel(IProductService productService, IMessenger messenger)
    {
        _productService = productService;
        _messenger = messenger;
        LoadProductsCommand.Execute(null);
        _isDeleted = false;
    }

    [RelayCommand]
    private async Task LoadProductsAsync()
    {
        Page = 1;
        await FetchProductsAsync();
    }

    [RelayCommand]
    private async Task NextPageAsync()
    {
        if (!HasNextPage) return;
        Page++;
        await FetchProductsAsync();
    }

    [RelayCommand]
    private async Task PreviousPageAsync()
    {
        if (!HasPreviousPage) return;
        Page--;
        await FetchProductsAsync();
    }

    private async Task FetchProductsAsync()
    {
        IsLoading = true;

        var queryParams = new ProductQueryParams
        {
            Page = Page,
            PageSize = PageSize,
            MinUnitPrice = MinUnitPrice,
            MaxUnitPrice = MaxUnitPrice,
            MinStockQuantity = MinStockQuantity,
            MaxStockQuantity = MaxStockQuantity,
            MinDiscountPercentage = MinDiscountPercentage,
            MaxDiscountPercentage = MaxDiscountPercentage,
            CategoryName = CategoryName,
            Description = Description,
            InStock = InStock,
            IsDeleted = IsDeleted
        };

        var result = await _productService.GetProductsForAdminAsync(queryParams);

        if (result is not null)
        {
            Products.Clear();

            foreach (var product in result.Data)
            {
                Products.Add(product);
            }

            TotalPages = result.TotalPages;
            HasNextPage = Page < TotalPages && result.TotalItemsRetrieved >= PageSize;
            HasPreviousPage = Page > 1;
        }

        IsLoading = false;
    }

    [RelayCommand]
    private async Task ClearFiltersAsync()
    {
        MinUnitPrice = null;
        MaxUnitPrice = null;
        MinStockQuantity = null;
        MaxStockQuantity = null;
        MinDiscountPercentage = null;
        MaxDiscountPercentage = null;
        CategoryName = null;
        Description = null;
        InStock = false;
        IsDeleted = false;
    }

    partial void OnSelectedProductChanged(ProductAdminData? value)
    {
        if (value is not null)
        {
            _messenger.Send(new ProductSelectedForAdminMessage(value.Id));
        }
    }
}
