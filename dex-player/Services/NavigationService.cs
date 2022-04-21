namespace DexPlayer.Services;

using DexPlayer.Values;
using DexPlayer.Views.Pages;
using DexPlayer.Views.UserControls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

internal interface INavigationService
{
    event Action<Type, object> Navigated;

    void NavigateTo(string pageId, object parameter = null);
    void NavigateTo(Type page, object parameter = null);
    void NavigateBack();
    void NavigateForward();
    bool CanNavigateBack();
    bool CanNavigateForward();
    Type ResolvePageType(string pageId);
    Task ShowDialog(ContentDialog dialog);
    Task ShowExceptionDialog(Exception exception);
    Task ShowExceptionDialog(string message);
    void SetXamlRoot(XamlRoot root);
}

internal class NavigationService : INavigationService
{
    private XamlRoot root;

    public event Action<Type, object> Navigated;

    public void NavigateTo(string pageId, object parameter = null)
    {
        var page = ResolvePageType(pageId);
        NavigateTo(page, parameter);
    }

    public void NavigateTo(Type page, object parameter = null)
    {
        Navigated?.Invoke(page, parameter);
    }

    public void NavigateBack()
    {
        throw new NotImplementedException();
    }

    public void NavigateForward()
    {
        throw new NotImplementedException();
    }

    public bool CanNavigateBack()
    {
        throw new NotImplementedException();
    }

    public bool CanNavigateForward()
    {
        throw new NotImplementedException();
    }

    public Type ResolvePageType(string pageId) =>
        pageId switch
        {
            Pages.LIBRARY => typeof(LibraryPage),
            Pages.LIBRARY_TRACKS => typeof(LibraryTracksPage),
            _ => typeof(NotFoundPage)
        };

    public async Task ShowDialog(ContentDialog dialog)
    {
        dialog.XamlRoot = root;
        await dialog.ShowAsync();
    }

    public async Task ShowExceptionDialog(Exception exception)
    {
        var dialog = new ContentDialog()
        {
            XamlRoot = root,
            Title = "Ошибка 😢",
            Content = new ExceptionUserControl(exception),
            CloseButtonText = "ок бро",
            DefaultButton = ContentDialogButton.Close
        };

        await dialog.ShowAsync();
    }

    public async Task ShowExceptionDialog(string message)
    {
        var dialog = new ContentDialog()
        {
            XamlRoot = root,
            Title = "Ошибка 😢",
            Content = message,
            CloseButtonText = "ок бро",
            DefaultButton = ContentDialogButton.Close
        };

        await dialog.ShowAsync();
    }

    public void SetXamlRoot(XamlRoot root)
    {
        this.root = root;
    }
}
