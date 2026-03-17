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

    public DisplayAdminCategoryDetailViewModel(ICategoryService categoryService, int categoryId, IMessenger messenger)
    {
        _categoryService = categoryService;
        CategoryId = categoryId;
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

    public async Task GetCategoryAsync()
    {
        Category = await _categoryService.GetCategoryForAdminAsync(CategoryId);
        if (Category is not null)
        {
            LoadProducts(Category.Products);
        }
    }

    [RelayCommand]
    private void LoadProducts(List<ProductAdminData> products)
    {
        var pagedProducts = products.Skip((Page - 1) * PageSize)
            .Take(PageSize);

        foreach (var product in pagedProducts)
        {
            Products.Add(product);
        }
        TotalPages = (int)Math.Ceiling(products.Count / (double)PageSize);
    }

    [RelayCommand]
    private void NextPage(List<ProductAdminData> products)
    {
        if (!HasNextPage) return;
        Page++;
        LoadProducts(products);
    }

    [RelayCommand]
    private void PreviousPage(List<ProductAdminData> products)
    {
        if (!HasPreviousPage) return;
        Page--;
        LoadProducts(products);
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavingateBackToAllAdminCategoriesMessage());
    }
}
