﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Fateblade.Licenzeee.WPF.Converters
{
    internal class UsageTypeIdMatchToVisibility : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || value is not int id || parameter == null || parameter is not string paramString || !int.TryParse(paramString, out int idToMatch)) 
                return Visibility.Collapsed;

            return id == idToMatch ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
