using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Enums;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.CategoryMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.ProductMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Auth;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Category;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using ECommerce.Shared.TerrenceLGee.Enums.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class MainUserViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessenger _messenger;
    private readonly IAuthService _authService;
    public ObservableCollection<MenuItemViewModel> MenuItems { get; } = [];

    [ObservableProperty]
    private ObservableObject? _currentSubView;

    [ObservableProperty]
    private MenuItemViewModel? _selectedMenuItem;

    public event Action? LogoutRequested;

    public MainUserViewModel(bool IsAdmin, IServiceProvider serviceProvider, IAuthService authService, IMessenger messenger)
    {
        _serviceProvider = serviceProvider;
        _authService = authService;
        _messenger = messenger;

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

        MessageRegistration();
    }

    partial void OnSelectedMenuItemChanged(MenuItemViewModel? value)
    {
        if (value is null) return;

        switch (value.Value)
        {
            case AdminMenu.AddCategory:
                CurrentSubView = _serviceProvider.GetRequiredService<AddCategoryViewModel>();
                break;
            case AdminMenu.UpdateCategory:
                CurrentSubView = _serviceProvider.GetRequiredService<AdminChooseCategoryForUpdateViewModel>();
                break;
            case AdminMenu.ViewCategories:
                CurrentSubView = _serviceProvider.GetRequiredService<ViewCategoriesForAdminViewModel>();
                break;
            case AdminMenu.AddProduct:
                CurrentSubView = _serviceProvider.GetRequiredService<AddProductViewModel>();
                break;
            case AdminMenu.UpdateProduct:
                CurrentSubView = _serviceProvider.GetRequiredService<AdminChooseProductForUpdateViewModel>();
                break;
            case AdminMenu.DeleteProduct:
                break;
            case AdminMenu.RestoreProduct:
                break;
            case AdminMenu.ViewProducts:
                CurrentSubView = _serviceProvider.GetRequiredService<ViewProductsForAdminViewModel>();
                break;
            case AdminMenu.ViewCustomers:
                break;
            case AdminMenu.ViewAllSales:
                break;
            case AdminMenu.UpdateSaleStatus:
                break;
            case AdminMenu.ViewAddresses:
                break;
            case CustomerMenu.ViewProfile:
                break;
            case CustomerMenu.ViewCategories:
                CurrentSubView = _serviceProvider.GetRequiredService<ViewCategoriesForCustomerViewModel>();
                break;
            case CustomerMenu.ViewProducts:
                break;
            case CustomerMenu.AddSale:
                break;
            case CustomerMenu.ViewOrders:
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
            case AdminMenu.Logout:
            case CustomerMenu.Logout:
                _authService.LogoutUserAsync();
                _messenger.UnregisterAll(this);
                LogoutRequested?.Invoke();
                break;
        }
    }

    private void MessageRegistration()
    {
        CategoryMessageRegistration();
        ProductMessageRegistration();
    }

    private void CategoryMessageRegistration()
    {
        _messenger.Register<CategoryAddedMessage>(this, (r, m) =>
        {
            CurrentSubView = new DisplayAddedCategoryViewModel(m.Data, _messenger);
        });

        _messenger.Register<NavigateBackToAddCategoryMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<AddCategoryViewModel>();
        });

        _messenger.Register<CategoryUpdatedMessage>(this, (r, m) =>
        {
            CurrentSubView = new DisplayUpdatedCategoryViewModel(m.Data, _messenger);
        });

        _messenger.Register<CategorySelectedForUpdateMessage>(this, (r, m) =>
        {
            var categoryService = _serviceProvider.GetRequiredService<ICategoryService>();
            CurrentSubView = new UpdateCategoryViewModel(categoryService, m.Data, _messenger);
        });

        _messenger.Register<NavigateBackToUpdateCategoryMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<AdminChooseCategoryForUpdateViewModel>();
        });

        _messenger.Register<CategorySelectedForAdminMessage>(this, async (r, m) =>
        {
            var categoryService = _serviceProvider.GetRequiredService<ICategoryService>();
            var detailVM = new DisplayAdminCategoryDetailViewModel(categoryService, m.CategoryId, _messenger);
            await detailVM.GetCategoryAsync();
            CurrentSubView = detailVM;
        });

        _messenger.Register<NavigateBackToAllAdminCategoriesMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<ViewCategoriesForAdminViewModel>();
        });

        _messenger.Register<CategorySelectedForCustomerMessage>(this, async (r, m) =>
        {
            var categoryService = _serviceProvider.GetRequiredService<ICategoryService>();
            var detailVM = new DisplayCustomerCategoryDetailViewModel(categoryService, m.CategoryId, _messenger);
            await detailVM.GetCategoryAsync();
            CurrentSubView = detailVM;
        });

        _messenger.Register<NavigateToAllCustomerCategoriesMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<ViewCategoriesForCustomerViewModel>();
        });
    }

    private void ProductMessageRegistration()
    {
        _messenger.Register<ProductAddedMessage>(this, (r, m) =>
        {
            CurrentSubView = new DisplayAddedProductViewModel(m.Data, _messenger);
        });

        _messenger.Register<NavigateBackToAddProductMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<AddProductViewModel>();
        });

        _messenger.Register<ProductSelectedForUpdateMessage>(this, (r, m) =>
        {
            var productService = _serviceProvider.GetRequiredService<IProductService>();
            CurrentSubView = new UpdateProductViewModel(productService, m.Data, _messenger);
        });

        _messenger.Register<ProductUpdatedMessage>(this, (r, m) =>
        {
            CurrentSubView = new DisplayUpdatedProductViewModel(m.Data, _messenger);
        });

        _messenger.Register<NavigateBackToUpdateProductMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<AdminChooseProductForUpdateViewModel>();
        });

        _messenger.Register<NavigateBackToAllAdminProductsMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<ViewProductsForAdminViewModel>();
        });

        _messenger.Register<ProductSelectedForAdminMessage>(this, async (r, m) =>
        {
            var productService = _serviceProvider.GetRequiredService<IProductService>();
            var detailVM = new DisplayAdminProductViewModel(productService, m.ProductId, _messenger);
            await detailVM.GetProductAsync();
            CurrentSubView = detailVM;
        });
    }
}
