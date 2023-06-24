using Fateblade.Licenzee.Db;
using Fateblade.Licenzee.Db.Models;
using Prism.Ioc;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Fateblade.Licenzeee.WPF.Converters
{
    internal class LicenseToUsageInfoString : IValueConverter
    {
        private readonly IDbProvider _dbProvider;

        public LicenseToUsageInfoString()
        {
            if (Application.Current is not IContainerApp)
            {
                throw new ArgumentException($"App does not implement '{nameof(IContainerApp)}'");
            }

            _dbProvider = (Application.Current as IContainerApp)!.Container.Resolve<IDbProvider>();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not License license) { return string.Empty; }

            switch (license.UsageType)
            {
                case UsageType.Comment: return license.UsageComment??string.Empty;
                case UsageType.SingleUser: return _dbProvider.Db.Users.FirstOrDefault(t=>t.Id ==license.LicenseUserId)?.Name ??string.Empty;
                case UsageType.MultiUser: return string.Join("; ", _dbProvider.Db.GetUsersOfLicense(license.Id).Select((t)=>t.Name));
                default: return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
