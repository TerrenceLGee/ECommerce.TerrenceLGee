using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Address;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Sale;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Customer;

public class CustomerData
{
    [JsonPropertyName("customerId")]
    public string CustomerId { get; set; } = string.Empty;

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; } = string.Empty;

    [JsonPropertyName("dateOfBirth")]
    public DateOnly DateOfBirth { get; set; }

    [JsonPropertyName("registrationDate")]
    public string RegistrationDate { get; set; } = string.Empty;

    [JsonPropertyName("addresses")]
    public List<AddressProfileData> Addresses { get; set; } = [];

    [JsonPropertyName("totalAmountOfAllSales")]
    public double TotalAmountOfAllSales { get; set; }

    [JsonPropertyName("sales")]
    public List<SaleForCustomerProfileData> Sales { get; set; } = [];
}