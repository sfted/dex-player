namespace DexPlayer.Views.Pages;

using DexPlayer.MVVM.Interfaces;
using DexPlayer.ViewModels.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

internal sealed partial class CollectionPage : Page, IViewModel<ICollectionVM>
{
    public CollectionPage()
    {
        ViewModel = App.Current.Services.GetService<ICollectionVM>();

        InitializeComponent();
    }

    public ICollectionVM ViewModel { get; set; }
}
