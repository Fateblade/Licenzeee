using Fateblade.Licenzee.Db.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fateblade.Licenzeee.WPF
{
    internal class EnumListProvider
    {
        public ObservableCollection<UsageType> UsageTypes { get; }

        public EnumListProvider()
        {
            UsageTypes = new ObservableCollection<UsageType>(Enum.GetNames<UsageType>().Select(Enum.Parse<UsageType>));
        }
    }
}
