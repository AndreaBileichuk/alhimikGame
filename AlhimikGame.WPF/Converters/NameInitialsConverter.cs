using System;
using System.Globalization;
using System.Windows.Data;

namespace AlhimikGame.WPF.Converters
{
    public class ProgressBarValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue && parameter is int maxValue && maxValue > 0)
            {
                return Math.Min(100, (double)intValue / maxValue * 100);
            }
            else if (value is int currentValue && parameter is string maxValueStr)
            {
                if (int.TryParse(maxValueStr, out int maxVal) && maxVal > 0)
                {
                    return Math.Min(100, (double)currentValue / maxVal * 100);
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NameInitialsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string name && !string.IsNullOrWhiteSpace(name))
            {
                string[] nameParts = name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (nameParts.Length == 1)
                {
                    return nameParts[0].Substring(0, 1).ToUpper();
                }
                else if (nameParts.Length > 1)
                {
                    return $"{nameParts[0].Substring(0, 1)}{nameParts[nameParts.Length - 1].Substring(0, 1)}".ToUpper();
                }
            }

            return "?";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}