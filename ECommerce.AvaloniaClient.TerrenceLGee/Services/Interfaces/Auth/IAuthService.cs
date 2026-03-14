using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Auth;
using ECommerce.Shared.TerrenceLGee.DTOs.AuthDTOs;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Auth;

public interface IAuthService
{
    Task<string?> RegisterUserAsync(UserRegistrationDto userDto);
    Task<AuthData?> LoginUserAsync(UserLoginDto userDto);
    Task<string?> LogoutAsync(AuthData data);
}
