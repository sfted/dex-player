namespace DexPlayer.Views.Pages;

using DesktopKit.MVVM.Interfaces;
using DexPlayer.ViewModels.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

internal sealed partial class LibraryPage : Page, IViewModel<ILibraryVM>
{
    public LibraryPage()
    {
        ViewModel = App.Current.Services.GetService<ILibraryVM>();

        InitializeComponent();
    }

    public ILibraryVM ViewModel { get; set; }
}
