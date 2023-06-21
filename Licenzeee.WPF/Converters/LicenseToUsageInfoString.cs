using Fateblade.Licenzee.Db.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Fateblade.Licenzeee.WPF.Converters
{
    internal class LicenseToUsageInfoString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not License license) {return string.Empty;}

            switch (license.UsageType)
            {
                case UsageType.Comment: return license.UsageComment??string.Empty;
                case UsageType.SingleUser: return InMemoryDb.Instance.Users.FirstOrDefault(t=>t.Id ==license.LicenseUserId)?.Name ??string.Empty;
                case UsageType.MultiUser: return string.Join("; ", InMemoryDb.Instance.GetUsersOfLicense(license.Id).Select((t)=>t.Name));
                default: return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
