using DesktopKit.Services;
using DexPlayer.Services;
using DexPlayer.Values;
using DexPlayer.ViewModels.Views.Pages;
using DexPlayer.ViewModels.Views.Windows;
using DexPlayer.Views.Pages;
using DexPlayer.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;

namespace DexPlayer;

public partial class App : Application
{
    public App()
    {
        Services = ConfigureServices();
        ConfigureNavigation(Services.GetRequiredService<INavigationService>());
        InitializeComponent();
    }

    private MainWindow mainWindow;

    public new static App Current => (App)Application.Current;
    public IServiceProvider Services { get; private set; }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        mainWindow = new MainWindow();
        mainWindow.Activate();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IYandexService, YandexService>();

        services.AddTransient<IMainWindowVM, MainWindowVM>();
        services.AddTransient<ILibraryVM, LibraryVM>();
        services.AddTransient<ILibraryTracksVM, LibraryTracksVM>();

        return services.BuildServiceProvider();
    }

    private static void ConfigureNavigation(INavigationService navigation)
    {
        navigation.PageTypes = new Dictionary<string, Type>
        {
            { Pages.LIBRARY, typeof(LibraryPage) },
            { Pages.LIBRARY_TRACKS, typeof(LibraryTracksPage) }
        };

        navigation.NotFoundPage = typeof(NotFoundPage);
    }
}
