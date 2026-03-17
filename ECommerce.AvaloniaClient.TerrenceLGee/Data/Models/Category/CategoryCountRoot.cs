using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Category;

public class CategoryCountRoot : Root
{
    [JsonPropertyName("data")]
    public int Data { get; set; }
}