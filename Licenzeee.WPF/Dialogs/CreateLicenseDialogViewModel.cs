using System;
using System.Collections.ObjectModel;
using System.Linq;
using Fateblade.Licenzee.Db.Models;
using Fateblade.Licenzeee.WPF.Events;
using Fateblade.Licenzeee.WPF.LookUpContracts;
using Prism.Commands;
using Prism.Events;

namespace Fateblade.Licenzeee.WPF.Dialogs
{
    internal class CreateLicenseDialogViewModel : LicenseDialogBaseViewModel
    {
        private readonly ShowCreateDialog<License> _dialogInfo;
        

        public CreateLicenseDialogViewModel(IEventAggregator eventAggregator, ShowCreateDialog<License> dialogInfo) : base(eventAggregator, dialogInfo)
        {
            _dialogInfo = dialogInfo;

            Products = new ObservableCollection<Product>(InMemoryDb.Instance.Products);

            Users = new ObservableCollection<User>(InMemoryDb.Instance.Users);

            Confirm = new DelegateCommand(createAndCloseDialog, ()=>SelectedProduct!=null&&!string.IsNullOrWhiteSpace(Key))
                .ObservesProperty(()=>SelectedProduct)
                .ObservesProperty(() => Key);
            Abort = new DelegateCommand(()=>
            {
                CloseDialog();
                _dialogInfo.AbortedCallback();
            });

            LookUpContract = new PersonLookUpContract();
        }


        private void createAndCloseDialog()
        {
            if (SelectedProduct == null) { return;}

            User[] users;
            if (SelectedUsageType == UsageType.SingleUser)
            {
                users = SelectedUser != null ? new[] { SelectedUser } : Array.Empty<User>();
            }
            else if (SelectedUsageType == UsageType.MultiUser)
            {
                users = SelectedUsers.ToArray();
            }
            else
            {
                users = Array.Empty<User>();
            }

            var createdLicense = InMemoryDb.Instance.CreateLicense(Key, SelectedProduct.Id, SelectedUsageType, UsageComment, users);

            CloseDialog();
            _dialogInfo.CompletedCallback(createdLicense);
        }
    }
}
