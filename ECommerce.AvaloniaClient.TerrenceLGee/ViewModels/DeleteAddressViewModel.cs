using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Address;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Address;
using ECommerce.Shared.TerrenceLGee.Parameters.AddressParameters;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DeleteAddressViewModel : ObservableObject
{
    private readonly IAddressService _addressService;

    public ObservableCollection<AddressData> Addresses { get; set; } = [];

    public DeleteAddressViewModel(IAddressService addressService)
    {
        _addressService = addressService;
        LoadAddressesCommand.Execute(null);
    }

    [ObservableProperty]
    private AddressData? _selectedAddress;

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
    private string? _successMessage;
    [ObservableProperty]
    private string? _errorMessage;

    [RelayCommand]
    private async Task LoadAddressesAsync()
    {
        IsLoading = true;

        var queryParams = new AddressQueryParams
        {
            Page = Page,
            PageSize = PageSize
        };

        var result = await _addressService.GetAddressesForCustomerAsync(queryParams);

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
    private async Task NextPageAsync()
    {
        if (!HasNextPage) return;
        Page++;
        await LoadAddressesAsync();
    }

    [RelayCommand]
    private async Task PreviousPageAsync()
    {
        if (!HasPreviousPage) return;
        Page--;
        await LoadAddressesAsync();
    }

    async partial void OnSelectedAddressChanged(AddressData? value)
    {
        SuccessMessage = null;
        ErrorMessage = null;

        if (value is not null)
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard("Delete", $"Delete Address {value.Id}?", ButtonEnum.YesNo, Icon.Warning,
                null, WindowStartupLocation.CenterOwner);

            var result = await box.ShowAsync();

            if (result == ButtonResult.Yes)
            {
                var (success, data) = await _addressService.DeleteAddressAsync(value.Id);

                if (success)
                {
                    SelectedAddress = null;
                    SuccessMessage = data;
                }
                else
                {
                    SelectedAddress = null;
                    ErrorMessage = data;
                }
                await LoadAddressesAsync();
            }
        }
    }
}
