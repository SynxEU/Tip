using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MauiApp4.Converters
{
    public class StarsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int r)
            {
                int rating = Math.Max(0, Math.Min(5, r));
                return new string('★', rating) + new string('☆', 5 - rating);
            }
            return "☆☆☆☆☆";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                int count = 0;
                foreach (var c in s)
                    if (c == '★') count++;
                return count;
            }
            return 0;
        }
    }
}
