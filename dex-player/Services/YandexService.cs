namespace DexPlayer.Services;

using DesktopKit.MVVM;
using DesktopKit.Services;
using DexPlayer.Exceptions;
using DexPlayer.Views.UserControls;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yandex.Music.Api;
using Yandex.Music.Api.Common;
using Yandex.Music.Api.Models.Account;
using Yandex.Music.Api.Models.Library;
using Yandex.Music.Api.Models.Track;

internal interface IYandexService
{
    event Action AccountInfoLoaded;

    YAccount Account { get; }

    Task InitializeApi();
    Task<YLibraryTracks> GetLikedTracks();
    Task<List<YTrack>> GetTracks(string[] ids);
}

internal class YandexService : ViewModelBase, IYandexService
{
    public YandexService(
        IAuthService authService,
        INavigationService navigationService)
    {
        this.authService = authService;
        this.navigationService = navigationService;

        loginCommand = new(ProceedLogin);

        api = new YandexMusicApi();
    }

    readonly IAuthService authService;
    readonly INavigationService navigationService;
    readonly Command<LoginUserControl> loginCommand;

    readonly AuthStorage storage = new();
    readonly YandexMusicApi api;

    public event Action AccountInfoLoaded;

    public YAccount Account { get; private set; }

    public async Task InitializeApi()
    {
        try
        {
            authService.LoadToken();
            await api.User.AuthorizeAsync(storage, authService.Token);
            await LoadAccountInfo();
        }
        catch (TokenNotFoundException)
        {
            var control = new LoginUserControl();

            var dialog = new ContentDialog()
            {
                Title = "🔑 Вход в Яндекс.Музыку",
                Content = control,
                PrimaryButtonText = "Войти",
                DefaultButton = ContentDialogButton.Primary,
                PrimaryButtonCommand = loginCommand,
                PrimaryButtonCommandParameter = control
            };

            await navigationService.ShowDialogAsync(dialog);
        }
        catch (Exception ex)
        {
            // TODO: вынести куда-нибудь создание этого диалога
            // TODO: вынести текст куда-нибудь в константы или словарь ресурсов
            await navigationService.ShowDialogAsync(new ContentDialog()
            {
                Title = "Ошибка 😢",
                Content = new ExceptionUserControl(ex),
                CloseButtonText = "ок бро",
                DefaultButton = ContentDialogButton.Close
            });
        }
    }

    public async Task<YLibraryTracks> GetLikedTracks() =>
        (await api.Library.GetLikedTracksAsync(storage)).Result;

    public async Task<List<YTrack>> GetTracks(string[] ids)
    {
        var result = await api.Track.GetAsync(storage, ids);
        var result2 = result.Result;
        return result2;
    }

    private async void ProceedLogin(LoginUserControl control)
    {
        var username = control.Username;
        var password = control.Password;

        try
        {
            await api.User.AuthorizeAsync(storage, username, password);
            authService.SaveToken(storage.Token);

            if (string.IsNullOrEmpty(authService.Token))
                await InitializeApi();
            else
                await LoadAccountInfo();
        }
        catch
        {
            await navigationService.ShowDialogAsync(new ContentDialog()
            {
                Title = "Ошибка 😢",
                Content = "Скорее всего, логин или пароль не подходят.",
                CloseButtonText = "ок бро",
                DefaultButton = ContentDialogButton.Close
            });

            await InitializeApi();
        }
    }

    private async Task LoadAccountInfo()
    {
        Account = (await api.User.GetUserAuthAsync(storage)).Result.Account;
        AccountInfoLoaded?.Invoke();
    }
}
