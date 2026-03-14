using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Auth;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class AuthViewModel : ObservableValidator
{
    private readonly IAuthService _authService;
    public event Action<bool>? LoginSuccessful;
    public event Action<bool>? LogoutSuccessful;

    public AuthViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [ObservableProperty]
    [Required(ErrorMessage = "First name is required.")]
    [NotifyPropertyChangedFor(nameof(FirstNameErrors))]
    private string _firstName = string.Empty;

    public string? FirstNameErrors => GetErrors(nameof(FirstName))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Last name is required.")]
    [NotifyPropertyChangedFor(nameof(LastNameErrors))]
    private string _lastName = string.Empty;

    public string? LastNameErrors => GetErrors(nameof(LastName))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [NotifyPropertyChangedFor(nameof(EmailAddressErrors))]
    private string _email = string.Empty;

    public string? EmailAddressErrors => GetErrors(nameof(Email))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Date of birth is required.")]
    [NotifyPropertyChangedFor(nameof(SelectedBirthDateErrors))]
    private DateTimeOffset? _selectedBirthDate;

    public string? SelectedBirthDateErrors => GetErrors(nameof(SelectedBirthDate))
        .FirstOrDefault()?.ErrorMessage;

    public DateOnly? DateOfBirth => SelectedBirthDate.HasValue
        ? DateOnly.FromDateTime(SelectedBirthDate.Value.Date)
        : null;

    [ObservableProperty]
    [Required(ErrorMessage = "Address Line 1 is required.")]
    [NotifyPropertyChangedFor(nameof(BillingAddressLine1Errors))]
    private string _billingAddressLine1;

    public string? BillingAddressLine1Errors => GetErrors(nameof(BillingAddressLine1))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    private string? _billingAddressLine2;

    [ObservableProperty]
    [Required(ErrorMessage = "City is required.")]
    [MaxLength(ErrorMessage = "City cannot exceed 50 characters.")]
    [NotifyPropertyChangedFor(nameof(BillingAddressCityErrors))]
    private string _billingAddressCity;

    public string? BillingAddressCityErrors => GetErrors(nameof(BillingAddressCity))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "State is required.")]
    [MaxLength(ErrorMessage = "State cannot exceed 50 characters.")]
    [NotifyPropertyChangedFor(nameof(BillingAddressStateErrors))]
    private string _billingAddressState;

    public string? BillingAddressStateErrors => GetErrors(nameof(BillingAddressState))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Postal code is required.")]
    [RegularExpression(@"\d{4,6}$", ErrorMessage = "Invalid postal code.")]
    [NotifyPropertyChangedFor(nameof(BillingAddressPostalCodeErrors))]
    private string _billingAddressPostalCode;

    public string? BillingAddressPostalCodeErrors => GetErrors(nameof(BillingAddressPostalCode))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Country is required.")]
    [MaxLength(ErrorMessage = "Country cannot exceed 50 characters.")]
    [NotifyPropertyChangedFor(nameof(BillingAddressCountryErrors))]
    private string _billingAddressCountry;

    public string? BillingAddressCountryErrors => GetErrors(nameof(BillingAddressCountryErrors))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Address Line 1 is required.")]
    [NotifyPropertyChangedFor(nameof(ShippingAddressLine1Errors))]
    private string _shippingAddressLine1;

    public string? ShippingAddressLine1Errors => GetErrors(nameof(ShippingAddressLine1))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    private string? _shippingAddressLine2;

    [ObservableProperty]
    [Required(ErrorMessage = "City is required.")]
    [MaxLength(ErrorMessage = "City cannot exceed 50 characters.")]
    [NotifyPropertyChangedFor(nameof(ShippingAddressCityErrors))]
    private string _shippingAddressCity;

    public string? ShippingAddressCityErrors => GetErrors(nameof(ShippingAddressCity))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "State is required.")]
    [MaxLength(ErrorMessage = "State cannot exceed 50 characters.")]
    [NotifyPropertyChangedFor(nameof(ShippingAddressStateErrors))]
    private string _shippingAddressState;

    public string? ShippingAddressStateErrors => GetErrors(nameof(ShippingAddressState))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Postal code is required.")]
    [RegularExpression(@"\d{4,6}$", ErrorMessage = "Invalid postal code.")]
    [NotifyPropertyChangedFor(nameof(ShippingAddressPostalCodeErrors))]
    private string _shippingAddressPostalCode;

    public string? ShippingAddressPostalCodeErrors => GetErrors(nameof(ShippingAddressPostalCode))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Country is required.")]
    [MaxLength(ErrorMessage = "Country cannot exceed 50 characters.")]
    [NotifyPropertyChangedFor(nameof(ShippingAddressCountryErrors))]
    private string _shippingAddressCountry;

    public string? ShippingAddressCountryErrors => GetErrors(nameof(ShippingAddressCountryErrors))
    .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    private bool _isBillingAddress;

    [ObservableProperty]
    private bool _isShippingAddress;

    [ObservableProperty]
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [NotifyPropertyChangedFor(nameof(PasswordErrors))]
    private string _password = string.Empty;

    public string? PasswordErrors => GetErrors(nameof(Password))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "You must confirm your password.")]
    [CustomValidation(typeof(AuthViewModel), nameof(ValidatePasswordConfirmation))]
    [NotifyPropertyChangedFor(nameof(ConfirmPasswordErrors))]
    private string _confirmPassword = string.Empty;

    public string? ConfirmPasswordErrors => GetErrors(nameof(ConfirmPassword))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    private string? _successMessage;

    [ObservableProperty]
    private string? _errorMessage;

    [RelayCommand]
    private async Task Register()
    {

    }

    public static ValidationResult? ValidatePasswordConfirmation(string confirmPassword, ValidationContext context)
    {
        var viewModel = (AuthViewModel)context.ObjectInstance;

        var password = viewModel.Password;

        if (!password.Equals(confirmPassword))
        {
            return new ValidationResult("Passwords do not match.");
        }

        return ValidationResult.Success;
    }
}