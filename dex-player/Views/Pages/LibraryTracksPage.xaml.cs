namespace DexPlayer.Views.Pages;

using DexPlayer.MVVM.Interfaces;
using DexPlayer.ViewModels.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

internal sealed partial class LibraryTracksPage : Page, IViewModel<ILibraryTracksVM>
{
    public LibraryTracksPage()
    {
        ViewModel = App.Current.Services.GetService<ILibraryTracksVM>();
        InitializeComponent();
    }

    public ILibraryTracksVM ViewModel { get; set; }

    private async void ListViewLoaded(object sender, RoutedEventArgs e)
    {
        var listView = sender as ListView;
        await ViewModel.InitializeTracks();
        // хз почему, но так и не удалось завести привязку
        listView.ItemsSource = ViewModel.Tracks;
    }
}
