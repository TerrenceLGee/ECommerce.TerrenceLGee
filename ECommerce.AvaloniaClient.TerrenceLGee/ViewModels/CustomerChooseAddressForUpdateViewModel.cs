using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Address;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class CustomerChooseAddressForUpdateViewModel : ObservableObject
{
    private readonly IAddressService _addressService;
    private readonly IMessenger _messenger;
}
