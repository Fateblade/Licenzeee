using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Fateblade.Licenzee.Db.Models;

namespace Fateblade.Licenzeee.WPF.Converters
{
    internal class LicensedProductToCombinedInfoString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Product licensedProduct) return string.Empty;

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
            if (value is not int licensedProductId) return string.Empty;

            return LicensedProductToCombinedInfoString.GetCombinedInfoString(InMemoryDb.Instance.Products.First(t=>t.Id==licensedProductId));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
