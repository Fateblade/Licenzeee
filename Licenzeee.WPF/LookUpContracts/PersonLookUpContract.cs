using BlackPearl.Controls.Contract;
using System;
using Fateblade.Licenzee.Db.Models;

namespace Fateblade.Licenzeee.WPF.LookUpContracts
{
    internal class PersonLookUpContract : ILookUpContract
    {
        public bool SupportsNewObjectCreation => false;

        public object CreateObject(object sender, string searchString)
        {
            throw new NotImplementedException("Contract does not support object creation");
        }

        public bool IsItemEqualToString(object sender, object item, string searchString)
        {
            if (item is not User licenseUser) {return false;}

            return string.Compare(searchString, licenseUser.Name, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public bool IsItemMatchingSearchString(object sender, object item, string searchString)
        {
            if (item is not User licenseUser) {return false;}
            if (string.IsNullOrEmpty(searchString)) {return false;}

            return licenseUser.Name.ToLower().Contains(searchString.ToLower());
        }
    }
}
