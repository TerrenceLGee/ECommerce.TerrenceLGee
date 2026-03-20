using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.CategoryMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplaySelectedProductFromCustomerCategoryDetailViewModel : ObservableObject
{
    public int ProductId { get; }
    public int CategoryId { get; }

    [ObservableProperty]
    public ProductData? _product;

    private readonly IProductService _productService;
    private readonly IMessenger _messenger;

    public DisplaySelectedProductFromCustomerCategoryDetailViewModel(
        IProductService productService, 
        int productId, 
        int categoryId, 
        IMessenger messenger)
    {
        _productService = productService;
        ProductId = productId;
        CategoryId = categoryId;
        _messenger = messenger;
    }

    public async Task GetProductAsync()
    {
        Product = await _productService.GetProductAsync(ProductId);
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToCustomerCategoryDetailView(CategoryId));
    }
}
