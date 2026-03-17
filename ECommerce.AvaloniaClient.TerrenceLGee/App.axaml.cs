using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data;
using ECommerce.AvaloniaClient.TerrenceLGee.Services;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Handlers;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Auth;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Category;
using ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;
using ECommerce.AvaloniaClient.TerrenceLGee.Views;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Linq;

namespace ECommerce.AvaloniaClient.TerrenceLGee;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();

        LoggingSetup();

        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
        services.AddSingleton<IAuthTokenHolder, AuthTokenHolder>();
        services.AddTransient<AuthHeaderHandler>();

        services.AddHttpClient("client", c =>
        {
            c.BaseAddress = new Uri(Urls.BaseUrl);
            c.DefaultRequestHeaders.Accept.Add(new("application/json"));
        })
            .AddHttpMessageHandler<AuthHeaderHandler>();

        services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<ICategoryService, CategoryService>();

        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<AuthViewModel>();
        services.AddTransient<MainUserViewModel>();
        services.AddTransient<AddCategoryViewModel>();
        services.AddTransient<DisplayAddedCategoryViewModel>();
        services.AddTransient<DisplayUpdatedCategoryViewModel>();
        services.AddTransient<DisplayAdminCategoryDetailViewModel>();
        services.AddTransient<ViewCategoriesForAdminViewModel>();
        services.AddTransient<AdminChooseCategoryForUpdateViewModel>();
        services.AddTransient<UpdateCategoryViewModel>();

        var serviceProvider = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();

            var mainWindowViewModel = serviceProvider
                .GetRequiredService<MainWindowViewModel>();

            ShowAuthView();

            void ShowAuthView()
            {
                var authViewModel = serviceProvider.GetRequiredService<AuthViewModel>();
                authViewModel.LoginSuccessful += OnLoginSuccessful;
                mainWindowViewModel.CurrentView = authViewModel;
            }

            void OnLoginSuccessful(bool isAdmin)
            {
                var authService = serviceProvider.GetRequiredService<IAuthService>();
                var messenger = serviceProvider.GetRequiredService<IMessenger>();
                var mainUserViewModel = new MainUserViewModel(isAdmin, serviceProvider, authService, messenger);
                mainUserViewModel.LogoutRequested += OnLogoutRequested;

                mainWindowViewModel.CurrentView = mainUserViewModel;
            }

            void OnLogoutRequested()
            {
                ShowAuthView();
            }

            desktop.MainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }

    void LoggingSetup()
    {
        var loggingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        Directory.CreateDirectory(loggingDirectory);
        var filePath = Path.Combine(loggingDirectory, "app-.txt");
        var outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File(
            path: filePath,
            rollingInterval: RollingInterval.Day,
            outputTemplate: outputTemplate)
            .CreateLogger();
    }
}