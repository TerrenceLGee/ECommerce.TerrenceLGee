using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using ECommerce.Shared.TerrenceLGee.Parameters.ProductParameters;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class RestoreProductViewModel : ObservableObject
{
    private readonly IProductService _productService;
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
    private string? _categoryName;
    [ObservableProperty]
    private bool? _isDeleted;

    [ObservableProperty]
    private string? _successMessage;
    [ObservableProperty]
    private string? _errorMessage;

    public RestoreProductViewModel(IProductService productService)
    {
        _productService = productService;
        LoadProductsCommand.Execute(null);
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
            CategoryName = CategoryName,
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
        CategoryName = string.Empty;
        IsDeleted = false;
    }

    async partial void OnSelectedProductChanged(ProductAdminData? value)
    {
        SuccessMessage = null;
        ErrorMessage = null;

        if (value is not null)
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard("Restore", $"Restore {value.Name}?", ButtonEnum.YesNo, Icon.Question,
                null, WindowStartupLocation.CenterOwner);

            var result = await box.ShowAsync();

            if (result == ButtonResult.Yes)
            {
                var (success, data) = await _productService.RestoreProductAsync(value.Id);

                if (success)
                {
                    SelectedProduct = null;
                    SuccessMessage = data;
                }
                else
                {
                    SelectedProduct = null;
                    ErrorMessage = data;
                }
                await FetchProductsAsync();
            }
        }
    }
}
