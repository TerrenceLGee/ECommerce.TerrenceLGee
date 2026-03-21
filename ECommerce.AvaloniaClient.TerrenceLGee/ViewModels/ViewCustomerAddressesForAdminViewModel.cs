using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Address;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.AddressMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Address;
using ECommerce.Shared.TerrenceLGee.Parameters.AddressParameters;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class ViewCustomerAddressesForAdminViewModel : ObservableObject
{
    private readonly IAddressService _addressService;
    private readonly IMessenger _messenger;
    public ObservableCollection<AddressData> Addresses { get; } = [];

    [ObservableProperty]
    private bool _isLoading;
    [ObservableProperty]
    private AddressData? _selectedAddress;

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
    private string? _city;
    [ObservableProperty]
    private string? _state;
    [ObservableProperty]
    private string? _country;

    public ViewCustomerAddressesForAdminViewModel(IAddressService addressService, IMessenger messenger)
    {
        _addressService = addressService;
        _messenger = messenger;
        LoadAddressesCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadAddressesAsync()
    {
        Page = 1;
        await FetchAddressesAsync();
    }

    [RelayCommand]
    private async Task NextPageAsync()
    {
        if (!HasNextPage) return;
        Page++;
        await FetchAddressesAsync();
    }

    [RelayCommand]
    private async Task PreviousPageAsync()
    {
        if (!HasPreviousPage) return;
        Page--;
        await FetchAddressesAsync();
    }

    private async Task FetchAddressesAsync()
    {
        IsLoading = true;

        var queryParams = new AddressQueryParams
        {
            Page = Page,
            PageSize = PageSize,
            City = City,
            State = State,
            Country = Country
        };

        var result = await _addressService.GetAllCustomerAddressesForAdminAsync(queryParams);

        if (result is not null)
        {
            Addresses.Clear();

            foreach (var address in result.Data)
            {
                Addresses.Add(address);
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
        City = string.Empty;
        State = string.Empty;
        Country = string.Empty;
    }

    partial void OnSelectedAddressChanged(AddressData? value)
    {
        if (value is not null)
        {
            _messenger.Send(new CustomerAddresseSelectedForAdminMessage(value.Id, value.CustomerId));
        }
    }
}
