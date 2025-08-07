using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace AlhimikGame.WPF.Converters;
public class CompletedLevelColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isCompleted)
        {
            return isCompleted ? Brushes.Green : Brushes.White;
        }
        return Brushes.White;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
