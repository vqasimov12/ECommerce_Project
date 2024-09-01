using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ECommerce_Project.Converters;
public class CoverImageBorderConverter:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var coverImage = parameter as string; 
        var imagePath = value as string;

        return imagePath == coverImage ? new Thickness(3) : new Thickness(1);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
       return null;
    }
}
