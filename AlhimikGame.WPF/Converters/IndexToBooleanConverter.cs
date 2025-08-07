using System.Globalization;
using System.Windows.Data;

namespace AlhimikGame.WPF.Converters;

public class IndexToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return false;

        try
        {
            int selectedIndex = (int)value;
            int currentIndex = System.Convert.ToInt32(parameter);

            return selectedIndex == currentIndex;
        }
        catch
        {
            return false;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return Binding.DoNothing;

        bool isChecked = (bool)value;
        if (isChecked)
        {
            try
            {
                return System.Convert.ToInt32(parameter);
            }
            catch
            {
                return Binding.DoNothing;
            }
        }

        return Binding.DoNothing;
    }
}