namespace DexPlayer.ViewModels.Views.Windows;

using DesktopKit.MVVM;
using DesktopKit.Services;
using DexPlayer.Services;

internal interface IMainWindowVM
{
    public IYandexService YandexService { get; }
    public INavigationService NavigationService { get; }
    public string Username { get; }
    
    public Command GoBackCommand { get; }
    public Command GoForwardCommand { get; }
}

internal class MainWindowVM : ViewModelBase, IMainWindowVM
{
    public MainWindowVM(IYandexService yandexService, INavigationService navigationService)
    {
        YandexService = yandexService;
        NavigationService = navigationService;

        GoBackCommand = new(NavigationService.NavigateBack, CanNavigateBack);
        GoForwardCommand = new(NavigationService.NavigateForward, CanNavigateForward);

        YandexService.AccountInfoLoaded += OnAccountInfoLoaded;
        NavigationService.Navigated += OnNavigationNavigated;
    }

    private string username = string.Empty;

    public IYandexService YandexService { get; private set; }
    public INavigationService NavigationService { get; private set; }

    public string Username
    {
        get => username;
        set => SetProperty(ref username, value);
    }

    public Command GoBackCommand { get; private set; }
    public Command GoForwardCommand { get; private set; }

    private void OnAccountInfoLoaded() =>
        Username = YandexService.Account.DisplayName;

    private void OnNavigationNavigated(System.Type page, object parameter)
    {
        GoBackCommand.RaiseCanExecuteChanged();
        GoForwardCommand.RaiseCanExecuteChanged();
    }

    private bool CanNavigateBack(object obj) => NavigationService.CanNavigateBack();
    private bool CanNavigateForward(object obj) => NavigationService.CanNavigateForward();
}
