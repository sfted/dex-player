namespace DexPlayer.ViewModels.Views.Windows;

using DesktopKit.Services;
using DexPlayer.MVVM;
using DexPlayer.Services;

internal interface IMainWindowVM
{
    public IYandexService YandexService { get; }
    public INavigationService NavigationService { get; }
    public string Username { get; }
}

internal class MainWindowVM : ViewModelBase, IMainWindowVM
{
    public MainWindowVM(IYandexService yandexService, INavigationService navigationService)
    {
        YandexService = yandexService;
        NavigationService = navigationService;

        YandexService.AccountInfoLoaded += OnAccountInfoLoaded;
    }

    private string username = string.Empty;

    public IYandexService YandexService { get; private set; }
    public INavigationService NavigationService { get; private set; }

    public string Username
    {
        get => username;
        set => SetProperty(ref username, value);
    }

    private void OnAccountInfoLoaded() =>
        Username = YandexService.Account.DisplayName;
}
