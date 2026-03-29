using System.ComponentModel.DataAnnotations;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Enums;

public enum CustomerMenu
{
    [Display(Name = "My Information")]
    ViewProfile,
    [Display(Name = "Shop")]
    AddSale,
    [Display(Name = "View all orders")]
    ViewOrders,
    [Display(Name = "Logout")]
    Logout
}
