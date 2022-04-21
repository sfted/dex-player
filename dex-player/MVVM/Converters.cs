namespace DexPlayer.MVVM;

using Microsoft.UI.Xaml.Data;
using System;

internal class DurationToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) =>
        TimeSpan.FromMilliseconds((long)value).ToString(@"mm\:ss");

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        throw new NotImplementedException();
}
