using System.ComponentModel.DataAnnotations;

namespace ECommerce.Entities.TerrenceLGee.Models;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Category name is required.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Category description cannot exceed 500 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Creation time is required.")]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public ICollection<Product> Products { get; set; } = [];
}