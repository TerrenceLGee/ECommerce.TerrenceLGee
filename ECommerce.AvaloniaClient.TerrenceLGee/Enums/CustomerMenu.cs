using System.ComponentModel.DataAnnotations;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Enums;

public enum CustomerMenu
{
    [Display(Name = "View profile")]
    ViewProfile,
    [Display(Name = "Shop")]
    AddSale,
    [Display(Name = "View all orders")]
    ViewOrders,
    [Display(Name = "View address info")]
    ViewAddresses,
    [Display(Name = "Logout")]
    Logout
}
