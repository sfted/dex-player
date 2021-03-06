namespace DexPlayer.ViewModels.Views.Pages;

using DesktopKit.MVVM;
using DesktopKit.Services;
using DexPlayer.Services;
using System.Diagnostics;
using System.Linq;

internal interface ILibraryVM
{
    string Username { get; }
    Command OpenTracksCommand { get; }
    Command OpenAlbumsCommand { get; }
    Command OpenArtistsCommand { get; }
    Command OpenPlaylistsCommand { get; }
    Command OpenPodcastsCommand { get; }
}

internal class LibraryVM : ViewModelBase, ILibraryVM
{
    public LibraryVM(
        IYandexService yandexService,
        INavigationService navigationService)
    {
        this.yandexService = yandexService;
        this.navigationService = navigationService;

        OpenTracksCommand = new(OpenTracks);

        if (yandexService.Account == null)
            yandexService.AccountInfoLoaded += () =>
                Username = yandexService.Account.DisplayName;
        else
            Username = yandexService.Account.DisplayName;
    }

    readonly IYandexService yandexService;
    readonly INavigationService navigationService;

    string username;

    public string Username
    {
        get => username;
        set => SetProperty(ref username, value);
    }

    public Command OpenTracksCommand { get; private set; }
    public Command OpenAlbumsCommand { get; private set; }
    public Command OpenArtistsCommand { get; private set; }
    public Command OpenPlaylistsCommand { get; private set; }
    public Command OpenPodcastsCommand { get; private set; }

    private async void OpenTracks()
    {
        var liked = (await yandexService.GetLikedTracks()).Library.Tracks.Take(10);

        //Debug.WriteLine(string.Join(", ", (await yandexService.GetTracks(string.Join(',', liked.Select(l => $"{l.Id}:{l.AlbumId}")))).Select(t => t.Title)));
        Debug.WriteLine(string.Join(", ",
            (await yandexService.GetTracks(
                liked.Select(l => $"{l.Id}:{l.AlbumId}").ToArray())
            ).Select(t => t.Title)));
    }
}
