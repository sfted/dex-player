namespace DexPlayer.ViewModels.Views.Pages;

using DesktopKit.MVVM;
using DexPlayer.Helpers;
using DexPlayer.Services;
using Microsoft.UI.Dispatching;
using System.Threading.Tasks;

internal interface ILibraryTracksVM
{
    TracksLazyObsCol Tracks { get; }

    Task InitializeTracks();
}

internal class LibraryTracksVM : ViewModelBase, ILibraryTracksVM
{
    public LibraryTracksVM(IYandexService yandexService)
    {
        this.yandexService = yandexService;
    }

    readonly IYandexService yandexService;

    public TracksLazyObsCol Tracks { get; private set; }

    public async Task InitializeTracks()
    {
        var lib = await yandexService.GetLikedTracks();
        Tracks = new(yandexService, lib, DispatcherQueue.GetForCurrentThread());
        NotifyPropertyChanged(nameof(Tracks));
    }
}
