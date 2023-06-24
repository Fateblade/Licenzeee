using Fateblade.Licenzee.Db;
using Fateblade.Licenzee.Db.Models;
using Fateblade.Licenzeee.WPF.Db;
using Prism.Ioc;
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
        private readonly IDbProvider _dbProvider;

        public LicensedProductIdToCombinedInfoString()
        {
            if (Application.Current is not IContainerApp)
            {
                throw new ArgumentException($"App does not implement '{nameof(IContainerApp)}'");
            }

            _dbProvider = (Application.Current as IContainerApp)!.Container.Resolve<IDbProvider>();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not int licensedProductId)
            {
                return string.Empty;
            }

            return LicensedProductToCombinedInfoString.GetCombinedInfoString(_dbProvider.Db.Products.First(t=>t.Id==licensedProductId));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
