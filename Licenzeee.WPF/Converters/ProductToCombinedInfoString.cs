using Fateblade.Licenzee.Db;
using Fateblade.Licenzee.Db.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Fateblade.Licenzeee.WPF.Converters
{
    internal class LicensedProductToCombinedInfoString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Product licensedProduct)
            {
                return string.Empty;
            }

            return GetCombinedInfoString(licensedProduct);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        internal static string GetCombinedInfoString(Product licensedProduct)
        {
            return $"{licensedProduct.Name} ({licensedProduct.Version})";
        }
    }

    internal class LicensedProductIdToCombinedInfoString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not int licensedProductId)
            {
                return string.Empty;
            }

            if (Application.Current is not IDbProvidingApp)
            {
                throw new ArgumentException($"App does not implement '{nameof(IDbProvidingApp)}'");
            }
            var db = (Application.Current as IDbProvidingApp)!.Db;

            return LicensedProductToCombinedInfoString.GetCombinedInfoString(db.Products.First(t=>t.Id==licensedProductId));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
