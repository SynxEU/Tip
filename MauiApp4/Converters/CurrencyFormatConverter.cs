using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MauiApp4.Converters
{
    public class CurrencyFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var useCulture = culture ?? CultureInfo.CurrentCulture;

            if (value == null) return string.Empty;

            var s = value as string;
            if (s != null && (s.Contains(useCulture.NumberFormat.CurrencySymbol) || s.IndexOfAny(new char[] { '$', '€', '£', '¥' }) >= 0))
                return s;

            if (value is double d)
                return d.ToString("C", useCulture);

            if (s != null && double.TryParse(s, NumberStyles.Any, useCulture, out var parsed))
                return parsed.ToString("C", useCulture);

            return s ?? value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
