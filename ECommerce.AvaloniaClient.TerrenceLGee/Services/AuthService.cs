using ECommerce.AvaloniaClient.TerrenceLGee.Data;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Auth;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Auth;
using ECommerce.Shared.TerrenceLGee.DTOs.AuthDTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services;

public class AuthService : IAuthService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<AuthService> _logger;
    private readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
    private string _errorMessage = string.Empty;
    private const string ClientName = "client";
    private const string MediaType = "application/json";
    private const string Scheme = "Bearer";
    private const string LogErrorString = "{msg}\n\n";

    public AuthService(IHttpClientFactory clientFactory, ILogger<AuthService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    } 

    public async Task<string?> RegisterUserAsync(UserRegistrationDto userDto)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.RegisterUrl}";

            var content = new StringContent(JsonSerializer.Serialize(userDto), Encoding.UTF8, MediaType);

            var response = await httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                return $"Unable to register new user:\nReason: {response.ReasonPhrase}.";
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var registrationResponse = JsonSerializer.Deserialize<RegistrationRoot>(responseContent, options);

            if (registrationResponse is null) return "Unable to register user at this time.";

            if (!registrationResponse.IsSuccess || registrationResponse.StatusCode != 200)
            {
                var errors = string.Join('\n', registrationResponse.Errors);
                return errors;
            }

            return registrationResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {nameof(RegisterUserAsync)}\n" +
                $"There was an API error attempting to register a new user: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return "Registration request failed.";
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {nameof(RegisterUserAsync)}\n" +
                $"There was an unexpected error attempting to register a new user: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return "Unexpected error during user registration\nRegistration failed";
        }
    }

    public async Task<AuthData?> LoginUserAsync(UserLoginDto userDto)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.LoginUrl}";

            var content = new StringContent(JsonSerializer.Serialize(userDto), Encoding.UTF8, MediaType);

            var response = await httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                return new AuthData
                {
                    ErrorMessage = $"Login failed\nReason: {response.ReasonPhrase}."
                };
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<AuthRoot>(responseContent, options);

            if (loginResponse is null)
            {
                return new AuthData
                {
                    ErrorMessage = "Unable to login at this time."
                };
            }

            if (loginResponse.Data is null)
            {
                return new AuthData
                {
                    ErrorMessage = $"Unable to retrieve valid authorization credientals."
                };
            }

            return loginResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {nameof(LoginUserAsync)}\n" +
                $"There was an API error during the user's login attempt: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return new AuthData
            {
                ErrorMessage = $"Error occurred during connection to the endpoint\nLogin failed."
            };
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {nameof(LoginUserAsync)}\n" +
                $"There was an unexpected error during the user's login attempt: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return new AuthData
            {
                ErrorMessage = $"Unable to connect\nLogin failed."
            };
        }
    }

    public async Task<string?> LogoutAsync(AuthData data)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.LogoutUrl}";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Scheme, data.AccessToken);

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return $"Error logging out\nReason: {response.ReasonPhrase}.";
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var logoutResponse = JsonSerializer.Deserialize<LogoutRoot>(responseContent, options);

            if (logoutResponse is null) return "Logout not possible.";

            if (!logoutResponse.IsSuccess || logoutResponse.StatusCode != 200)
            {
                return string.Join('\n', logoutResponse.Errors);
            }

            return logoutResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {nameof(LogoutAsync)}\n" +
                $"There was an API error logging the user out of the system: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return "Error during logout attempt";
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {nameof(LogoutAsync)}\n" +
                $"There was an unexpected error logging the user out of the system: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return "Unable to connect\nLogout failed";
        }
    }
}