namespace DexPlayer;

using DexPlayer.Services;
using DexPlayer.ViewModels.Views.Pages;
using DexPlayer.ViewModels.Views.Windows;
using DexPlayer.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;

public partial class App : Application
{
    public App()
    {
        Services = ConfigureServices();

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
        services.AddTransient<ICollectionVM, CollectionVM>();

        return services.BuildServiceProvider();
    }
}
