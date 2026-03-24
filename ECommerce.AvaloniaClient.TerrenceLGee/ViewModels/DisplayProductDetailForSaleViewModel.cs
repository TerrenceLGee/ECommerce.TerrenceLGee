using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.SaleMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using ECommerce.Shared.TerrenceLGee.DTOs.OrderDTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayProductDetailForSaleViewModel : ObservableObject
{
    private readonly IProductService _productService;
    private readonly IMessenger _messenger;
    private readonly int _categoryId;

    [ObservableProperty]
    private ProductData _product;
    [ObservableProperty]
    private static List<CartItemDto> _shoppingCart;
    [ObservableProperty]
    private decimal _quantity = 1;
    [ObservableProperty]
    public decimal _maxQuantity = 5000;
    [ObservableProperty]
    private string? _successMessage;
    [ObservableProperty]
    private string? _errorMessage;



    public DisplayProductDetailForSaleViewModel(
        IProductService productService,
        List<CartItemDto> shoppingCart,
        ProductData product,
        int categoryId,
        IMessenger messenger)
    {
        _productService = productService;
        _product = product;
        _shoppingCart = shoppingCart;
        _categoryId = categoryId;
        _messenger = messenger;
    }

    [RelayCommand]
    private async Task AddToCartAsync()
    {
        ShoppingCart.Add(new CartItemDto { ProductId = Product.Id, Quantity = (int)Quantity, ProductName = Product.Name });
        SuccessMessage = $"Successfully added item to your shopping cart";
    }

    [RelayCommand]
    private async Task RemoveFromCartAsync()
    {
        var itemToRemove = ShoppingCart.Where(ci => ci.ProductId == Product.Id && ci.Quantity == (int)Quantity)
            .FirstOrDefault();

        if (itemToRemove is not null)
        {
            ShoppingCart.Remove(itemToRemove);
            SuccessMessage = "Item successfully removed from your shopping cart";
        }
        else
        {
            ErrorMessage = "Error removed item from your shopping cart";
        }
    }

    [RelayCommand]
    private void GoBackToProducts()
    {
        _messenger.Send(new NavigateBackToProductsFromSelectedProductMessage(_categoryId, ShoppingCart));
    }

    [RelayCommand]
    private void Checkout()
    {
        _messenger.Send(new CheckoutFromProductDetail(ShoppingCart));
    }
}
