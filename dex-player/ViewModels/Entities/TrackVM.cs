namespace DexPlayer.ViewModels.Entities;

using DesktopKit.MVVM;
using DesktopKit.MVVM.Interfaces;
using DexPlayer.Helpers.Abstractions.Interfaces;
using Yandex.Music.Api.Models.Track;

internal class TrackVM : ViewModelBase, IModel<YTrack>, ISupportsNumeration
{
    public TrackVM(YTrack track)
    {
        Model = track;
    }

    public YTrack Model { get; set; }
    public int Number { get; set; }
}
