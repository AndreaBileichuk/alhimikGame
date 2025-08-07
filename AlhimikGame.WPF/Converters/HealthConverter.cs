using System.Globalization;
using System.Windows.Data;

namespace AlhimikGame.WPF.Converters;

public class HealthConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 2 && values[0] is int current && values[1] is int max)
        {
            return $"{current}/{max}";
        }
        return "N/A";
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException(); // не треба для OneWay
    }
}
