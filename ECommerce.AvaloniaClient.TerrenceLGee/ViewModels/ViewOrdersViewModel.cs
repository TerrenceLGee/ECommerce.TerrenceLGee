using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Sale;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.SaleMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Sale;
using ECommerce.Shared.TerrenceLGee.Enums;
using ECommerce.Shared.TerrenceLGee.Parameters.SaleParameters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class ViewOrdersViewModel : ObservableObject
{
    private readonly ISaleService _saleService;
    private readonly IMessenger _messenger;

    public ObservableCollection<SaleSummaryData> Sales { get; set; } = [];

    public ViewOrdersViewModel(ISaleService saleService, IMessenger messenger)
    {
        _saleService = saleService;
        _messenger = messenger;
        SaleStatuses = new List<SaleStatus>
        {
            SaleStatus.Pending,
            SaleStatus.Processing,
            SaleStatus.Shipped,
            SaleStatus.Delivered,
            SaleStatus.Canceled
        };
        LoadSalesCommand.Execute(null);
    }

    [ObservableProperty]
    private SaleSummaryData? _selectedSale;
    [ObservableProperty]
    private SaleStatus? _selectedStatus;

    [ObservableProperty]
    private bool _isLoading;
    [ObservableProperty]
    private int _page = 1;
    [ObservableProperty]
    private int _pageSize = 10;
    [ObservableProperty]
    private int _totalPages;
    [ObservableProperty]
    private bool _hasNextPage;
    [ObservableProperty]
    private bool _hasPreviousPage;
    [ObservableProperty]
    private decimal? _minTotalAmount;
    [ObservableProperty]
    private decimal? _maxTotalAmount;
    [ObservableProperty]
    private string? _status;

    [ObservableProperty]
    private List<SaleStatus> _saleStatuses;

    [RelayCommand]
    private async Task LoadSalesAsync()
    {
        Page = 1;
        await FetchSalesAsync();
    }

    [RelayCommand]
    private async Task NextPageAsync()
    {
        if (!HasNextPage) return;
        Page++;
        await FetchSalesAsync();
    }

    [RelayCommand]
    private async Task PreviousPageAsync()
    {
        if (!HasPreviousPage) return;
        Page--;
        await FetchSalesAsync();
    }

    [RelayCommand]
    public async Task ClearFilters()
    {
        MinTotalAmount = null;
        MaxTotalAmount = null;
        SelectedStatus = null;
    }

    private async Task FetchSalesAsync()
    {
        IsLoading = true;

        var queryParams = new SaleQueryParams
        {
            Page = Page,
            PageSize = PageSize,
            MinTotalAmount = MinTotalAmount,
            MaxTotalAmount = MaxTotalAmount,
            Status = (SelectedStatus.HasValue)
            ? SelectedStatus.Value.ToString()
            : null
        };

        var result = await _saleService.GetSalesForCustomerAsync(queryParams);

        if (result is not null)
        {
            Sales.Clear();

            foreach (var sale in result.Data)
            {
                Sales.Add(sale);
            }

            TotalPages = result.TotalPages;
            HasNextPage = Page < TotalPages && result.TotalItemsRetrieved >= PageSize;
            HasPreviousPage = Page > 1;
        }

        IsLoading = false;
    }

    partial void OnSelectedSaleChanged(SaleSummaryData? value)
    {
        if (value is not null)
        {
            _messenger.Send(new SaleSelectedForCustomerDetailMessage(value.Id));
        }
    }
}
