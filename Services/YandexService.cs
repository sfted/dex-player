namespace DexPlayer.Services;

using DexPlayer.Exceptions;
using DexPlayer.MVVM;
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
    Task<List<YTrack>> GetTracks(string id);
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

            await navigationService.ShowDialog(dialog);
        }
        catch (Exception ex)
        {
            await navigationService.ShowExceptionDialog(ex);
        }
    }

    public async Task<YLibraryTracks> GetLikedTracks() =>
        (await api.Library.GetLikedTracksAsync(storage)).Result;

    public async Task<List<YTrack>> GetTracks(string id) =>
        (await api.Track.GetAsync(storage, id)).Result;

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
            await navigationService.ShowExceptionDialog("Скорее всего, логин или пароль не подходят.");
            await InitializeApi();
        }
    }

    private async Task LoadAccountInfo()
    {
        Account = (await api.User.GetUserAuthAsync(storage)).Result.Account;
        AccountInfoLoaded?.Invoke();
    }
}
