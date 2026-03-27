using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Address;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.AddressMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.OtherMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Address;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayAddressViewModel : ViewModelBase
{
    private readonly IAddressService _addressService;
    private readonly IMessenger _messenger;
    public AddressData Address { get; }

    public DisplayAddressViewModel(IAddressService addressService, AddressData address, IMessenger messenger)
    {
        _addressService = addressService;
        Address = address;
        _messenger = messenger;
    }

    [ObservableProperty]
    private string? _successMessage;
    [ObservableProperty]
    private string? _errorMessage;

    [RelayCommand]
    private async Task GoBack()
    {
        _messenger.Send(new NavigateBackToPreviousPageMessage());
    }

    [RelayCommand]
    private void UpdateAddress()
    {
        _messenger.Send(new AddressSelectedForUpdateMessage(Address));
    }

    [RelayCommand]
    private async Task DeleteAddress()
    {
        SuccessMessage = null;
        ErrorMessage = null;

        var box = MessageBoxManager
                .GetMessageBoxStandard("Delete", $"Delete Address {Address.Id}?", ButtonEnum.YesNo, Icon.Warning,
                null, WindowStartupLocation.CenterOwner);

        var result = await box.ShowAsync();

        if (result == ButtonResult.Yes)
        {
            var (success, data) = await _addressService.DeleteAddressAsync(Address.Id);

            if (success)
            {
                SuccessMessage = data;
            }
            else
            {
                ErrorMessage = data;
            }
        }
    }
}
