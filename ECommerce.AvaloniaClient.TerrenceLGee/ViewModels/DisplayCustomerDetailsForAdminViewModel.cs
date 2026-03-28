using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Address;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Customer;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Sale;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.Customer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayCustomerDetailsForAdminViewModel : ObservableObject
{
    [ObservableProperty]
    private CustomerData _customer;
    private readonly IMessenger _messenger;

    public ObservableCollection<AddressProfileData> Addresses { get; } = [];
    public ObservableCollection<SaleForCustomerProfileData> Orders { get; } = [];

    [ObservableProperty]
    private List<AddressProfileData> _addressesForDisplay;
    [ObservableProperty]
    private List<SaleForCustomerProfileData> _ordersForDisplay;

    [ObservableProperty]
    private AddressProfileData? _selectedAddress;

    [ObservableProperty]
    private SaleForCustomerProfileData? _selectedOrder;

    [ObservableProperty]
    private int _addressPage = 1;
    [ObservableProperty]
    private int _addressPageSize = 10;
    [ObservableProperty]
    private int _addressTotalPages;
    [ObservableProperty]
    private bool _addressHasNextPage;
    [ObservableProperty]
    private bool _addressHasPreviousPage;

    [ObservableProperty]
    private int _orderPage = 1;
    [ObservableProperty]
    private int _orderPageSize = 10;
    [ObservableProperty]
    private int _orderTotalPages;
    [ObservableProperty]
    private bool _orderHasNextPage;
    [ObservableProperty]
    private bool _orderHasPreviousPage;

    public string CustomerName { get; set; }
    public DisplayCustomerDetailsForAdminViewModel(CustomerData customer, IMessenger messenger)
    {
        _customer = customer;
        _messenger = messenger;
        _addressesForDisplay = new List<AddressProfileData>();
        _ordersForDisplay = new List<SaleForCustomerProfileData>();
        LoadAddresses(_customer.Addresses);
        LoadOrders(_customer.Sales);
        FetchAddressesCommand.Execute(null);
        FetchOrdersCommand.Execute(null);
        CustomerName = $"{_customer.FirstName} {_customer.LastName}";
    }

    [RelayCommand]
    private void FetchAddresses()
    {
        var pagedAddresses = Addresses.Skip((AddressPage - 1) * AddressPageSize)
            .Take(AddressPageSize)
            .ToList();

        AddressesForDisplay = pagedAddresses;

        AddressTotalPages = (int)Math.Ceiling(Addresses.Count / (double)AddressPageSize);
        AddressHasNextPage = AddressPage < AddressTotalPages;
        AddressHasPreviousPage = AddressPage > 1;
    }

    [RelayCommand]
    private void NextAddressPage()
    {
        if (!AddressHasNextPage) return;
        AddressPage++;
        FetchAddresses();
    }

    [RelayCommand]
    private void PreviousAddressPage()
    {
        if (!AddressHasPreviousPage) return;
        AddressPage--;
        FetchAddresses();
    }

    [RelayCommand]
    private void FetchOrders()
    {
        var pagedOrders = Orders.Skip((OrderPage - 1) * OrderPageSize)
            .Take(OrderPageSize)
            .ToList();

        OrdersForDisplay = pagedOrders;

        OrderTotalPages = (int)Math.Ceiling(Orders.Count / (double)OrderPageSize);
        OrderHasNextPage = OrderPage < OrderTotalPages;
        OrderHasPreviousPage = OrderPage > 1;
    }

    [RelayCommand]
    private void NextOrderPage()
    {
        if (!OrderHasNextPage) return;
        OrderPage++;
        FetchOrders();
    }

    [RelayCommand]
    private void PreviousOrderPage()
    {
        if (!OrderHasPreviousPage) return;
        OrderPage--;
        FetchOrders();
    }

    private void LoadAddresses(List<AddressProfileData> addresses)
    {
        foreach (var address in addresses)
        {
            Addresses.Add(address);
        }
    }

    private void LoadOrders(List<SaleForCustomerProfileData> orders)
    {
        foreach (var order in orders)
        {
            Orders.Add(order);
        }
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new ViewCustomersForAdminMessage());
    }

    partial void OnSelectedAddressChanged(AddressProfileData? value)
    {
        if (value is not null)
        {
            _messenger.Send(new DisplayCustomerAddressDetailForAdminMessage(value.AddressId, Customer.CustomerId));
        }
    }

    partial void OnSelectedOrderChanged(SaleForCustomerProfileData? value)
    {
        if (value is not null)
        {
            _messenger.Send(new AdminSelectedCustomerOrderForDetailMessage(value.Id, Customer));
        }
    }
}
