using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MauiApp4.Converters
{
    public class ZeroToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var useCulture = culture ?? CultureInfo.CurrentCulture;

            if (value == null) return false;

            if (value is double d) return Math.Abs(d) > 0.0001;

            var s = value as string;
            if (s != null)
            {
                var cleaned = s;
                var symbol = useCulture.NumberFormat.CurrencySymbol;
                if (!string.IsNullOrEmpty(symbol)) cleaned = cleaned.Replace(symbol, string.Empty);

                if (double.TryParse(cleaned, NumberStyles.Any, useCulture, out var parsed))
                    return Math.Abs(parsed) > 0.0001;
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
