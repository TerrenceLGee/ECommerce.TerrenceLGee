using ECommerce.Api.TerrenceLGee.Controllers.Helpers;
using ECommerce.Api.TerrenceLGee.Responses;
using ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.DTOs.AddressDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Api.TerrenceLGee.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddressesController : ControllerBase
{
    private readonly IAddressService _addressService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AddressesController(
        IAddressService addressService,
        UserManager<ApplicationUser> userManager)
    {
        _addressService = addressService;
        _userManager = userManager;
    }

    [HttpPost("add")]
    [Authorize(Roles = "admin,customer")]
    public async Task<ActionResult<ApiResponse<RetrievedAddressDto?>>> AddAddress([FromBody] CreateAddressDto address)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await AuthHelper.IsValidUserAsync<RetrievedAddressDto?>(_userManager, userId);

        if (!isValidUser)
        {
            return StatusCode(errorResponse.StatusCode, errorResponse);
        }

        address.CustomerId = userId;

        var result = await _addressService.AddAddressAsync(address);

        ApiResponse<RetrievedAddressDto?> response;

        if (result.IsFailure)
        {
            if (result.ErrorMessage!.Contains("Unable to add address"))
            {
                response = new ApiResponse<RetrievedAddressDto?>(400, [result.ErrorMessage]);
            }
            else
            {
                response = new ApiResponse<RetrievedAddressDto?>(500, [result.ErrorMessage]);
            }

            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedAddressDto?>(201, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("update/{id:int}")]
    [Authorize(Roles = "admin,customer")]
    public async Task<ActionResult<ApiResponse<RetrievedAddressDto?>>> UpdateAddress(
        [FromBody] UpdateAddressDto address, 
        [FromRoute] int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await AuthHelper.IsValidUserAsync<RetrievedAddressDto?>(_userManager, userId);

        if (!isValidUser)
        {
            return StatusCode(errorResponse.StatusCode, errorResponse);
        }

        address.Id = id;
        address.CustomerId = userId;

        var result = await _addressService.UpdateAddressAsync(address);

        ApiResponse<RetrievedAddressDto?> response;

        if (result.IsFailure)
        {
            if (result.ErrorMessage!.Contains("Unable to update address"))
            {
                response = new ApiResponse<RetrievedAddressDto?>(400, [result.ErrorMessage]);
            }
            else
            {
                response = new ApiResponse<RetrievedAddressDto?>(500, [result.ErrorMessage]);
            }

            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedAddressDto?>(200, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "admin,customer")]
    public async Task<ActionResult<ApiResponse<string?>>> DeleteAddress([FromRoute] int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await AuthHelper.IsValidUserAsync<string?>(_userManager, userId);

        if (!isValidUser)
        {
            return StatusCode(errorResponse.StatusCode, errorResponse);
        }

        var addressIdDto = new AddressIdDto
        {
            Id = id,
            CustomerId = userId
        };

        var result = await _addressService.DeleteAddressAsync(addressIdDto);

        ApiResponse<string?> response;

        if (result.IsFailure)
        {
            if (result.ErrorMessage!.Contains("Unable to delete address"))
            {
                response = new ApiResponse<string?>(400, [result.ErrorMessage]);
            }
            else
            {
                response = new ApiResponse<string?>(500, [result.ErrorMessage]);
            }

            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<string?>(200, "Address deleted successfully");

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "admin,customer")]
    public async Task<ActionResult<RetrievedAddressDto?>> GetAddress([FromRoute] int id)
    {
        throw new NotImplementedException();
    }
}
