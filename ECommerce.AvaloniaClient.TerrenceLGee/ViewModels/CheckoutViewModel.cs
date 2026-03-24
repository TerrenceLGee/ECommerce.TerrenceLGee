using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.SaleMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Sale;
using ECommerce.Shared.TerrenceLGee.DTOs.OrderDTOs;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class CheckoutViewModel : ObservableObject
{
    private readonly ISaleService _saleService;
    private readonly IMessenger _messenger;

    [ObservableProperty]
    private static List<CartItemDto> _shoppingCart;
    [ObservableProperty]
    private static List<CartItemDto> _shoppingCartForDisplay;
    [ObservableProperty]
    private string? _successMessage;
    [ObservableProperty]
    private string? _errorMessage;
    [ObservableProperty]
    private CartItemDto? _selectedItem;

    public CheckoutViewModel(ISaleService saleService, List<CartItemDto> shoppingCart, IMessenger messenger)
    {
        _saleService = saleService;
        _messenger = messenger;
        _shoppingCart = shoppingCart;
        _shoppingCartForDisplay = new List<CartItemDto>();
        LoadCartCommand.Execute(null);
    }

    [RelayCommand]
    private void LoadCart()
    {
        ShoppingCartForDisplay = ShoppingCart;
    }

    [RelayCommand]
    private async Task CheckoutAsync()
    {
        SuccessMessage = null;
        ErrorMessage = null;

        var order = new CreateOrderDto
        {
            ShoppingCart = ShoppingCart
        };

        var result = await _saleService.CreateOrderAsync(order);

        if (result is null)
        {
            ErrorMessage = "Unable to complete order at this time";
            return;
        }

        if (string.IsNullOrEmpty(result.ErrorMessage))
        {
            SuccessMessage = "Order completed successfully";
            _messenger.Send(new OrderCompletedMessage(result));
        }
        else
        {
            ErrorMessage = result.ErrorMessage;
        }
    }

    [RelayCommand]
    private void CancelOrder()
    {
        _messenger.Send(new NavigateBackToAllCategoriesOrderCanceledMessage());
    }

    async partial void OnSelectedItemChanged(CartItemDto? value)
    {
        if (value is not null)
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard("Delete", "Delete this item?", ButtonEnum.YesNo, Icon.Warning, null, WindowStartupLocation.CenterOwner);

            var result = await box.ShowAsync();

            if (result == ButtonResult.Yes)
            {
                ShoppingCart.Remove(value);
                LoadCart();
            }
        }
    }
}
