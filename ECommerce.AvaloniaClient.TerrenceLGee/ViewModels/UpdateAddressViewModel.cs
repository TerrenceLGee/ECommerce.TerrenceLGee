using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.AddressMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Address;
using ECommerce.Shared.TerrenceLGee.DTOs.AddressDTOs;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class UpdateAddressViewModel : ObservableValidator
{
    private readonly IAddressService _addressService;
    private readonly IMessenger _messenger;
}
