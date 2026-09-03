using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MauiApp4.Converters
{
    public class NormalizeBillConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = (value as string) ?? string.Empty;
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var useCulture = culture ?? CultureInfo.CurrentCulture;
            if (double.TryParse(text, NumberStyles.Any, useCulture, out var parsed))
            {
                if (parsed < 0)
                    parsed = 0; 

                return parsed.ToString("G", useCulture);
            }

            return text;
        }
    }
}
