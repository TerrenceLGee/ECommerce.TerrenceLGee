using Avalonia.Remote.Protocol;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Sale;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Sale;
using System.Collections.ObjectModel;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class ViewOrdersViewModel : ObservableObject
{
    private readonly ISaleService _saleService;
    private readonly IMessenger _messenger;

    public ObservableCollection<SaleData> Sales { get; set; } = [];
}
