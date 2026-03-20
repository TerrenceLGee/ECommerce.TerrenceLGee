using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.ProductMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayCustomerProductViewModel : ObservableObject
{
    public int ProductId { get; }

    [ObservableProperty]
    public ProductData? _product;

    private readonly IProductService _productService;
    private readonly IMessenger _messenger;

    public DisplayCustomerProductViewModel(IProductService productService, int productId, IMessenger messenger)
    {
        _productService = productService;
        ProductId = productId;
        _messenger = messenger;
    }

    public async Task GetProductAsync()
    {
        Product= await _productService.GetProductAsync(ProductId);
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToAllCustomerProductsMessage());
    }
}
