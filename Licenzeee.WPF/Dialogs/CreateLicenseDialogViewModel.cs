using System;
using System.Collections.ObjectModel;
using System.Linq;
using Fateblade.Licenzeee.WPF.Events;
using Fateblade.Licenzeee.WPF.LookUpContracts;
using Fateblade.Licenzeee.WPF.Models;
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

            Products = new ObservableCollection<Product>(Db.Instance.Products);
            UsageTypes = new ObservableCollection<UsageType>(Db.Instance.UsageTypes);
            SelectedUsageType = UsageTypes.First();

            Users = new ObservableCollection<User>(Db.Instance.Users);

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
            if (SelectedUsageType.Id == 2)
            {
                users = SelectedUser != null ? new[] { SelectedUser } : Array.Empty<User>();
            }
            else if (SelectedUsageType.Id == 3)
            {
                users = SelectedUsers.ToArray();
            }
            else
            {
                users = Array.Empty<User>();
            }

            var createdLicense = Db.Instance.CreateLicense(Key, SelectedProduct.Id, SelectedUsageType.Id, UsageComment, users);

            CloseDialog();
            _dialogInfo.CompletedCallback(createdLicense);
        }
    }
}
