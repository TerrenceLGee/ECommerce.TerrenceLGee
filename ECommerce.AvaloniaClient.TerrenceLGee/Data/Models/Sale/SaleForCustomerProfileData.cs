using ECommerce.Shared.TerrenceLGee.Enums;
using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Sale;

public class SaleForCustomerProfileData
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("saleProductCount")]
    public int SaleProductCount { get; set; }

    [JsonPropertyName("totalBaseAmount")]
    public double TotalBaseAmount { get; set; }

    [JsonPropertyName("totalDiscountAmount")]
    public double TotalDiscountAmount { get; set; }

    [JsonPropertyName("totalAmount")]
    public double TotalAmount { get; set; }

    [JsonPropertyName("saleStatus")]
    public SaleStatus SaleStatus { get; set; }
}