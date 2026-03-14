using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;

public class ProductCountRoot : Root
{
    [JsonPropertyName("data")]
    public int Data { get; set; }
}