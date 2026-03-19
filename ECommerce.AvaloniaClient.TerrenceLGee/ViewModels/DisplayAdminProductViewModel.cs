using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.ProductMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayAdminProductViewModel : ObservableObject
{
    public int ProductId { get; }

    [ObservableProperty]
    public ProductAdminData? _product;

    private readonly IProductService _productService;
    private readonly IMessenger _messenger;

    public DisplayAdminProductViewModel(IProductService productService, int productId, IMessenger messenger)
    {
        _productService = productService;
        ProductId = productId;
        _messenger = messenger;
    }

    public async Task GetProductAsync()
    {
        Product = await _productService.GetProductForAdminAsync(ProductId);
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToAllAdminProductsMessage());
    }
}
