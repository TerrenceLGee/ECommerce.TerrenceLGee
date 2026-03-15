using System.ComponentModel.DataAnnotations;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Enums;

public enum AdminMenu
{
    [Display(Name = "Add a new category")]
    AddCategory,
    [Display(Name = "Update an existing category")]
    UpdateCategory,
    [Display(Name = "View all categories")]
    ViewCategories,
    [Display(Name = "View a category by id")]
    ViewCategoryById,
    [Display(Name = "View a category by name")]
    ViewCategoryByName,
    [Display(Name = "Add a new product")]
    AddProduct,
    [Display(Name = "Update an existing product")]
    UpdateProduct,
    [Display(Name = "Delete an existing product")]
    DeleteProduct,
    [Display(Name = "Restore a previously deleted product")]
    RestoreProduct,
    [Display(Name = "View all products")]
    ViewProducts,
    [Display(Name = "View product by id")]
    ViewProductById,
    [Display(Name = "View product by name")]
    ViewProductByName,
    [Display(Name = "View all customers")]
    ViewCustomers,
    [Display(Name = "View all customer sales")]
    ViewAllSales,
    [Display(Name = "View a sale by id")]
    ViewSaleById,
    [Display(Name = "Update a customer sale status")]
    UpdateSaleStatus,
    [Display(Name = "View all customer addresses")]
    ViewAddresses,
    [Display(Name = "Logout")]
    Logout
}
