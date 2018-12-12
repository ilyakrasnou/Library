using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using System.Globalization;

namespace Library.Converters 
{
    class DefaultImageSourse: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (File.Exists((string)value))
                return value;
            else
                return "image-default.png";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
