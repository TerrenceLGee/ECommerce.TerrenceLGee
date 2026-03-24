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
    [Display(Name = "Cancel order")]
    CancelOrder,
    [Display(Name = "Add a new address")]
    AddAddress,
    [Display(Name = "Update an existing address")]
    UpdateAddress,
    [Display(Name = "Delete an existing address")]
    DeleteAddress,
    [Display(Name = "View all of my addresses")]
    ViewAddresses,
    [Display(Name = "Logout")]
    Logout
}
