using ECommerce.Api.TerrenceLGee.Responses;
using ECommerce.Contracts.TerrenceLGee.Common.Results;
using ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Api.TerrenceLGee.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager)
    {
        _authService = authService;
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<string?>>> Register([FromBody] UserRegistrationDto userDto)
    {
        ApiResponse<string?> response;

        var result = await _authService.RegisterUserAsync(userDto);

        if (result.IsFailure)
        {
            if (result.ErrorMessage!.Contains("User already exists"))
            {
                response = new ApiResponse<string?>(409, [result.ErrorMessage]);
            }
            else if (result.ErrorMessage!.Contains("Unable to register user"))
            {
                response = new ApiResponse<string?>(400, [result.ErrorMessage]);
            }
            else
            {
                response = new ApiResponse<string?>(500, [result.ErrorMessage]);
            }

            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<string?>(200, "Registration successful");
        return StatusCode(response.StatusCode, response);
    }

    private ActionResult<ApiResponse<T?>> HandleFailure<T>(out ApiResponse<T?> response, Result<T> result)
    {
        if (result.ErrorMessage!.Contains("User already exists"))
        {
            response = new ApiResponse<T?>(409, [result.ErrorMessage]);
        }
        else if (result.ErrorMessage!.Contains("Unable to register user"))
        {
            response = new ApiResponse<T?>(400, [result.ErrorMessage]);
        }
        else
        {
            response = new ApiResponse<T?>(500, [result.ErrorMessage]);
        }

        return StatusCode(response.StatusCode, response);
    }
}
