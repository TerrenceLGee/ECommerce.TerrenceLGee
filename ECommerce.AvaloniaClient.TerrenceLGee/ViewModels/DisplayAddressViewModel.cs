using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Address;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.AddressMessages;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayAddressViewModel : ViewModelBase
{
    private readonly IMessenger _messenger;
    public AddressData Address { get; }

    public DisplayAddressViewModel(AddressData address, IMessenger messenger)
    {
        Address = address;
        _messenger = messenger;
    }

    [RelayCommand]
    private async Task GoBack()
    {
        _messenger.Send(new NavigateBackToAllAddressesMessage());
    }
}
