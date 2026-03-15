using CommunityToolkit.Mvvm.ComponentModel;
using ECommerce.AvaloniaClient.TerrenceLGee.Enums;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Auth;
using ECommerce.Shared.TerrenceLGee.Enums.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class MainUserViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IAuthService _authService;
    public ObservableCollection<MenuItemViewModel> MenuItems { get; } = [];

    [ObservableProperty]
    private ObservableObject? _currentSubView;

    [ObservableProperty]
    private MenuItemViewModel? _selectedMenuItem;

    public event Action? LogoutRequested;

    public MainUserViewModel(bool IsAdmin, IServiceProvider serviceProvider, IAuthService authService)
    {
        _serviceProvider = serviceProvider;
        _authService = authService;

        if (IsAdmin)
        {
            var adminMenuItems = Enum.GetValues<AdminMenu>()
                .Select(e => new MenuItemViewModel(e.GetDisplayName(), e));

            foreach (var item in adminMenuItems)
            {
                MenuItems.Add(item);
            }
        }
        else
        {
            var customerMenuItems = Enum.GetValues<CustomerMenu>()
                .Select(e => new MenuItemViewModel(e.GetDisplayName(), e));

            foreach (var item in customerMenuItems)
            {
                MenuItems.Add(item);
            }
        }
    }

    partial void OnSelectedMenuItemChanged(MenuItemViewModel? value)
    {
        if (value is null) return;

        switch (value.Value)
        {
            case AdminMenu.AddCategory:
                break;
            case AdminMenu.UpdateCategory:
                break;
            case AdminMenu.ViewCategories:
                break;
            case AdminMenu.ViewCategoryById:
                break;
            case AdminMenu.ViewCategoryByName:
                break;
            case AdminMenu.AddProduct:
                break;
            case AdminMenu.UpdateProduct:
                break;
            case AdminMenu.DeleteProduct:
                break;
            case AdminMenu.RestoreProduct:
                break;
            case AdminMenu.ViewProducts:
                break;
            case AdminMenu.ViewProductById:
                break;
            case AdminMenu.ViewProductByName:
                break;
            case AdminMenu.ViewCustomers:
                break;
            case AdminMenu.ViewAllSales:
                break;
            case AdminMenu.ViewSaleById:
                break;
            case AdminMenu.UpdateSaleStatus:
                break;
            case AdminMenu.ViewAddresses:
                break;
            case CustomerMenu.ViewProfile:
                break;
            case CustomerMenu.ViewCategories:
                break;
            case CustomerMenu.ViewCategoryById:
                break;
            case CustomerMenu.ViewCategoryByName:
                break;
            case CustomerMenu.ViewProducts:
                break;
            case CustomerMenu.ViewProductById:
                break;
            case CustomerMenu.ViewProductByName:
                break;
            case CustomerMenu.AddSale:
                break;
            case CustomerMenu.ViewOrders:
                break;
            case CustomerMenu.ViewOrderById:
                break;
            case CustomerMenu.CancelOrder:
                break;
            case CustomerMenu.AddAddress:
                break;
            case CustomerMenu.UpdateAddress:
                break;
            case CustomerMenu.DeleteAddress:
                break;
            case CustomerMenu.ViewAddresses:
                break;
            case CustomerMenu.ViewAddressById:
                break;
            case AdminMenu.Logout:
            case CustomerMenu.Logout:
                _authService.LogoutUserAsync();
                LogoutRequested?.Invoke();
                break;
        }
    }
}
