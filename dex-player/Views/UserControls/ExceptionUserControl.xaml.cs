namespace DexPlayer.Views.UserControls;

using Microsoft.UI.Xaml.Controls;
using System;

public sealed partial class ExceptionUserControl : UserControl
{
    public ExceptionUserControl(Exception exception)
    {
        Message = exception.Message;
        Details = exception.ToString();

        InitializeComponent();
    }

    public string Message { get; set; }
    public string Details { get; set; }
}
