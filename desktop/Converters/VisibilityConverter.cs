using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace desktop.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool booleanValue && booleanValue
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            return value is not bool booleanValue || !booleanValue
                ? Visibility.Collapsed
                : Visibility.Visible;
        }
    }
}
