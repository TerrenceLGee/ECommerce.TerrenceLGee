using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Sale;

public class SaleCountRoot : Root
{
    [JsonPropertyName("data")]
    public int Data { get; set; }
}