using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Category;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.CategoryMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Category;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayAdminCategoryDetailViewModel : ObservableObject
{
    public int CategoryId { get; }
    [ObservableProperty]
    public CategoryAdminData? _category;
    public ObservableCollection<ProductAdminData> Products { get; } = [];
    private readonly ICategoryService _categoryService;
    private readonly IMessenger _messenger;

    [ObservableProperty]
    private ProductAdminData? _selectedProduct;

    [ObservableProperty]
    private List<ProductAdminData> _productsForDisplay;

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

    public DisplayAdminCategoryDetailViewModel(ICategoryService categoryService, int categoryId, IMessenger messenger)
    {
        _categoryService = categoryService;
        CategoryId = categoryId;
        _messenger = messenger;
        _productsForDisplay = new List<ProductAdminData>();
    }

    public async Task GetCategoryAsync()
    {
        Category = await _categoryService.GetCategoryForAdminAsync(CategoryId);
        if (Category is not null)
        {
            LoadProducts(Category.Products);
            FetchProducts();
        }
    }

    
    private void LoadProducts(List<ProductAdminData> products)
    {
        foreach (var product in products)
        {
            Products.Add(product);
        }
    }

    [RelayCommand]
    private void FetchProducts()
    {
        var pagedProducts = Products.Skip((Page - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        ProductsForDisplay = pagedProducts;

        TotalPages = (int)Math.Ceiling(Products.Count / (double)PageSize);
        HasNextPage = Page < TotalPages;
        HasPreviousPage = Page > 1;
    }

    [RelayCommand]
    private void NextPage()
    {
        if (!HasNextPage) return;
        Page++;
        FetchProducts();
    }

    [RelayCommand]
    private void PreviousPage()
    {
        if (!HasPreviousPage) return;
        Page--;
        FetchProducts();
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToAllAdminCategoriesMessage());
    }

    partial void OnSelectedProductChanged(ProductAdminData? value)
    {
        if (value is not null)
        {
            _messenger.Send(new CategoryProductSelectedForAdminMessage(value.Id, CategoryId));
        }
    }
}
