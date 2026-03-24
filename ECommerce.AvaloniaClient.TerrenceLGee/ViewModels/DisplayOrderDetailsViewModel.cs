using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Sale;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.SaleMessages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayOrderDetailsViewModel : ViewModelBase
{
    [ObservableProperty]
    private SaleData _sale;
    public ObservableCollection<SaleProductData> OrderItems { get; } = [];
    private readonly IMessenger _messenger;

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

    public DisplayOrderDetailsViewModel(SaleData sale, IMessenger messenger)
    {
        _sale = sale;
        _messenger = messenger;
        LoadOrderItems(sale.SaleProducts);
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
    private void ShopAgain()
    {
        _messenger.Send(new ShopAgainMessage());
    }
}
