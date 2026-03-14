using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Customer;

public class CustomerCountAdminRoot : Root
{
    [JsonPropertyName("data")]
    public int Data { get; set; }
}