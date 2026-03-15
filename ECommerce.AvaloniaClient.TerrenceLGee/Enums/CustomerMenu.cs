using System.ComponentModel.DataAnnotations;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Enums;

public enum CustomerMenu
{
    [Display(Name = "View profile")]
    ViewProfile,
    [Display(Name = "View all categories")]
    ViewCategories,
    [Display(Name = "View category by id")]
    ViewCategoryById,
    [Display(Name = "View category by name")]
    ViewCategoryByName,
    [Display(Name = "View all products")]
    ViewProducts,
    [Display(Name = "View product by id")]
    ViewProductById,
    [Display(Name = "View product by name")]
    ViewProductByName,
    [Display(Name = "Shop")]
    AddSale,
    [Display(Name = "View all orders")]
    ViewOrders,
    [Display(Name = "View order by id")]
    ViewOrderById,
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
    [Display(Name = "View address by id")]
    ViewAddressById,
    [Display(Name = "Logout")]
    Logout
}
