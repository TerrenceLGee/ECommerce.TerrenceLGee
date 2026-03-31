using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.SaleMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using ECommerce.Shared.TerrenceLGee.DTOs.OrderDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.ProductParameters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class ViewProductsForSaleViewModel : ObservableObject
{
    private readonly IProductService _productService;
    private readonly IMessenger _messenger;
    private readonly int _categoryId;

    [ObservableProperty]
    private static List<CartItemDto> _shoppingCart;

    public ObservableCollection<ProductData> Products { get; } = [];

    [ObservableProperty]
    private bool _isLoading;
    [ObservableProperty]
    private ProductData? _selectedProduct;

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


    public ViewProductsForSaleViewModel(
        IProductService productService,
        int categoryId,
        List<CartItemDto> shoppingCart,
        IMessenger messenger)
    {
        _productService = productService;
        _categoryId = categoryId;
        _shoppingCart = shoppingCart;
        _messenger = messenger;
        LoadProductsCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadProductsAsync()
    {
        IsLoading = true;

        var queryParams = new ProductQueryParams
        {
            Page = Page,
            PageSize = PageSize,
            CategoryId = _categoryId
        };

        var result = await _productService.GetProductsAsync(queryParams);

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
    private async Task NextPageAsync()
    {
        if (!HasNextPage) return;
        Page++;
        await LoadProductsAsync();
    }

    [RelayCommand]
    private async Task PreviousPageAsync()
    {
        if (!HasPreviousPage) return;
        Page--;
        await LoadProductsAsync();
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToAllCategoriesForSale());
    }

    partial void OnSelectedProductChanged(ProductData? value)
    {
        if (value is not null)
        {
            _messenger.Send(new ProductSelectedForSaleMessage(value, _categoryId, ShoppingCart));
        }
    }

    [RelayCommand]
    private void ViewCart()
    {
        _messenger.Send(new ViewCartMessage(ShoppingCart));
    }

    [RelayCommand]
    private void Checkout()
    {
        _messenger.Send(new CheckoutMessage(ShoppingCart));
    }
}
