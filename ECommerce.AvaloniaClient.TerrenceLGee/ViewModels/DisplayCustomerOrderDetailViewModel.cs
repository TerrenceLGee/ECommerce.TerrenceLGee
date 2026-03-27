using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Sale;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.OtherMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.SaleMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Sale;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayCustomerOrderDetailViewModel : ObservableObject
{
    public int SaleId { get; }
    [ObservableProperty]
    private SaleData? _sale;

    public ObservableCollection<SaleProductData> OrderItems { get; } = [];
    private readonly ISaleService _saleService;
    private readonly IMessenger _messenger;

    [ObservableProperty]
    private SaleProductData? _selectedItem;

    public DisplayCustomerOrderDetailViewModel(ISaleService saleService, int saleId, IMessenger messenger)
    {
        _saleService = saleService;
        SaleId = saleId;
        _messenger = messenger;
    }

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
    private string? _successMessage;
    [ObservableProperty]
    private string? _errorMessage;

    public async Task GetSaleAsync()
    {
        Sale = await _saleService.GetSaleForCustomerAsync(SaleId);
        if (Sale is null) return;
        LoadOrderItems(Sale.SaleProducts);
    }

    [RelayCommand]
    private void LoadOrderItems(List<SaleProductData> items)
    {
        var pagedItems = items.Skip((Page - 1) * PageSize)
            .Take(PageSize);

        foreach (var item in pagedItems)
        {
            OrderItems.Add(item);
        }

        TotalPages = (int)Math.Ceiling(items.Count / (double)PageSize);
        HasNextPage = Page < TotalPages;
        HasPreviousPage = Page > 1;
    }

    [RelayCommand]
    private void NextPage(List<SaleProductData> items)
    {
        if (!HasNextPage) return;
        Page++;
        LoadOrderItems(items);
    }

    [RelayCommand]
    private void PreviousPage(List<SaleProductData> items)
    {
        if (!HasPreviousPage) return;
        Page--;
        LoadOrderItems(items);
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToPreviousPageMessage());
    }

    [RelayCommand]
    private async Task CancelOrderAsync()
    {
        SuccessMessage = null;
        ErrorMessage = null;

        var box = MessageBoxManager
            .GetMessageBoxStandard("Cancel", $"Cancel Order?", ButtonEnum.YesNo, Icon.Question,
            null, WindowStartupLocation.CenterOwner);

        var result = await box.ShowAsync();

        if (result == ButtonResult.Yes)
        {
            var (success, data) = await _saleService.CustomerCancelSaleAsync(SaleId);

            if (success)
            {
                SuccessMessage = data;
            }
            else
            {
                ErrorMessage = data;
            }
        }
    }

    partial void OnSelectedItemChanged(SaleProductData? value)
    {
        if (value is not null)
        {
            _messenger.Send(new SaleProductSelectedForCustomerDetailMessage(value.ProductId));
        }
    }
}
