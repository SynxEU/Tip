using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace MauiApp4.Converters
{
    public class TipPctToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double pct = 0;
            if (value is double d) pct = d;
            else if (value is string s && double.TryParse(s, NumberStyles.Any, culture ?? CultureInfo.CurrentCulture, out var parsed)) pct = parsed;

            if (pct >= 20) return Color.FromArgb("#D9534F");
            if (pct >= 10) return Color.FromArgb("#F0AD4E");
            return Color.FromArgb("#5CB85C"); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
