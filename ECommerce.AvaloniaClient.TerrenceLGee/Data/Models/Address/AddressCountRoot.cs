using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Address;

public class AddressCountRoot : Root
{
    [JsonPropertyName("data")]
    public int Data { get; set; }
}