namespace DexPlayer.Views.UserControls;

using global::Windows.System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using System;

public sealed partial class LoginUserControl : UserControl
{
    public LoginUserControl()
    {
        InitializeComponent();
    }

    public string Username { get; set; }
    public string Password { get; set; }

    private async void HyperlinkGo(Hyperlink sender, HyperlinkClickEventArgs args) =>
        await Launcher.LaunchUriAsync(sender.NavigateUri);
}
