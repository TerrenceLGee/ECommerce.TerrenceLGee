using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Address;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.AddressMessages;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayAddedAddressViewModel : ViewModelBase
{
    private readonly IMessenger _messenger;
    public AddressData Address { get; }

    public DisplayAddedAddressViewModel(AddressData address, IMessenger messenger)
    {
        Address = address;
        _messenger = messenger;
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToAddAddressMessage());
    }

    [RelayCommand]
    private void GoBackToAllAddresses()
    {
        _messenger.Send(new NavigateBackToAllAddressesAfterAddMessage());
    }

}
