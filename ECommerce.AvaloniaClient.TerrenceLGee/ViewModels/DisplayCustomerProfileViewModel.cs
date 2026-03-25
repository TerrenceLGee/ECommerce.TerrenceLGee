using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Address;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Customer;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Sale;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Customer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayCustomerProfileViewModel : ObservableObject
{
    private readonly ICustomerService _customerService;
    private readonly IMessenger _messenger;

    public ObservableCollection<AddressProfileData> Addresses { get; } = [];
    public ObservableCollection<SaleForCustomerProfileData> Orders { get; } = [];

    [ObservableProperty]
    private CustomerData? _profile;

    public DisplayCustomerProfileViewModel(ICustomerService customerService, IMessenger messenger)
    {
        _customerService = customerService;
        _messenger = messenger;
    }


    public async Task GetProfileAsync()
    {
        Profile = await _customerService.GetCustomerProfileAsync();

        if (Profile is not null)
        {
            LoadAddresses(Profile.Addresses);
            LoadOrders(Profile.Sales);
        }
    }

    [RelayCommand]
    private void LoadAddresses(List<AddressProfileData> addresses)
    {
        foreach (var address in addresses)
        {
            Addresses.Add(address);
        }
    }

    [RelayCommand]
    private void LoadOrders(List<SaleForCustomerProfileData> orders)
    {
        foreach (var order in orders)
        {
            Orders.Add(order);
        }
    }
}
