using ECommerce.Entities.TerrenceLGee.Models;
using System.Data;

namespace ECommerce.Tests.TerrenceLGee.Resources.CategoryResources;

public static class RepositoryResources
{
    private static readonly int CategoryId = 1;

    public static Category GetCategoryAfterAddSuccess()
    {
        return new()
        {
            Id = CategoryId,
            Name = "Outdoors",
            Description = "Everything you need to engage in a wide range of outdoor activities",
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Category GetCategoryAfterUpdateSuccess()
    {
        return new()
        {
            Id = CategoryId,
            Name = "Outdoors",
            Description = "For the avid outdoorsman in your life a wide range of exciting products to meet any endeavor",
            UpdatedAt = DateTime.UtcNow
        };
    }

    public static Category GetCategory()
    {
        return new()
        {
            Id = CategoryId + 1,
            Name = "Books",
            Description = "Books on almost every subject from A-Z.",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow.AddHours(8)
        };
    }
}
