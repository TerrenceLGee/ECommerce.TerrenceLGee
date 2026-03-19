using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Category;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.CategoryMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Category;
using ECommerce.Shared.TerrenceLGee.Parameters.CategoryParameters;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class ViewCategoriesForCustomerViewModel : ObservableObject
{
    private readonly ICategoryService _categoryService;
    private readonly IMessenger _messenger;
    public ObservableCollection<CategorySummaryData> Categories { get; } = [];

    [ObservableProperty]
    private bool _isLoading;
    [ObservableProperty]
    private CategorySummaryData? _selectedCategory;

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
    private string? _searchByDescription;

    public ViewCategoriesForCustomerViewModel(ICategoryService categoryService, IMessenger messenger)
    {
        _categoryService = categoryService;
        _messenger = messenger;
        LoadCategoriesCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadCategoriesAsync()
    {
        IsLoading = true;

        var queryParams = new CategoryQueryParams
        {
            Page = Page,
            PageSize = PageSize,
            Description = SearchByDescription
        };

        var result = await _categoryService.GetCategoriesAsync(queryParams);

        if (result is not null)
        {
            Categories.Clear();

            foreach (var category in result.Data)
            {
                Categories.Add(category);
            }

            TotalPages = result.TotalPages;
            HasNextPage = Page < TotalPages;
            HasPreviousPage = Page > 1;
        }

        IsLoading = false;
    }

    [RelayCommand]
    private async Task NextPageAsync()
    {
        if (!HasNextPage) return;
        Page++;
        await LoadCategoriesAsync();
    }

    [RelayCommand]
    private async Task PreviousPageAsync()
    {
        if (!HasPreviousPage) return;
        Page--;
        await LoadCategoriesAsync();
    }

    partial void OnSelectedCategoryChanged(CategorySummaryData? value)
    {
        if (value is not null)
        {
            _messenger.Send(new CategorySelectedForCustomerMessage(value.Id));
        }
    }
}
