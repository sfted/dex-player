namespace DexPlayer.Helpers;

using DexPlayer.Helpers.Abstractions;
using DexPlayer.Services;
using DexPlayer.ViewModels.Entities;
using Microsoft.UI.Dispatching;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Music.Api.Models.Library;
using Yandex.Music.Api.Models.Track;

internal class TracksLazyObsCol : LazyObservableCollection<YTrack, TrackVM>
{
    public TracksLazyObsCol(
        IYandexService yandexService,
        YLibraryTracks libraryTracks,
        DispatcherQueue dispatcherQueue) : base(dispatcherQueue)
    {
        this.yandexService = yandexService;
        this.libraryTracks = libraryTracks;
        max = (uint)libraryTracks.Library.Tracks.Count;
    }

    readonly IYandexService yandexService;
    readonly YLibraryTracks libraryTracks;

    public override async Task<List<YTrack>> LoadModels(uint count, uint offset) =>
        await yandexService.GetTracks(
            libraryTracks.Library.Tracks
                .Skip((int)offset)
                .Take((int)count)
                .Select(t => $"{t.Id}:{t.AlbumId}")
                .ToArray());

    public override TrackVM ToVM(YTrack model, int number) =>
        new(model) { Number = number };
}
