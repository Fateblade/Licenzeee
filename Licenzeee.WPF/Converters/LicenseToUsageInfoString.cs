using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Fateblade.Licenzee.Db.Models;

namespace Fateblade.Licenzeee.WPF.Converters
{
    internal class LicenseToUsageInfoString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not License license) {return string.Empty;}

            switch (license.UsageTypeId)
            {
                case 1: return license.UsageComment??string.Empty;
                case 2: return Db.Instance.Users.FirstOrDefault(t=>t.Id ==license.LicenseUserId)?.Name ??string.Empty;
                case 3: return string.Join("; ", Db.Instance.GetUsersOfLicense(license.Id).Select((t)=>t.Name));
                default: return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
