using System;
using System.Collections.ObjectModel;
using System.Linq;
using Fateblade.Licenzeee.WPF.Events;
using Fateblade.Licenzeee.WPF.LookUpContracts;
using Fateblade.Licenzeee.WPF.Models;
using Prism.Commands;
using Prism.Events;

namespace Fateblade.Licenzeee.WPF.Dialogs;

internal class ModifyLicenseDialogViewModel : LicenseDialogBaseViewModel
{
    private readonly ShowModifyDialog<License> _dialogInfo;


    public ModifyLicenseDialogViewModel(IEventAggregator eventAggregator, ShowModifyDialog<License> dialogInfo) : base(eventAggregator, dialogInfo)
    {
        _dialogInfo = dialogInfo;

        SelectedProduct = Products.First(t => t.Id == dialogInfo.ToModify.ProductId);
        SelectedUsageType = UsageTypes.First(t => t.Id == dialogInfo.ToModify.UsageTypeId);
        Key = dialogInfo.ToModify.Key;
        SelectedUser = Users.FirstOrDefault(t => t.Id == dialogInfo.ToModify.LicenseUserId);
        UsageComment = dialogInfo.ToModify.UsageComment ?? string.Empty;
        SelectedUsers = new ObservableCollection<User>(Db.Instance.GetUsersOfLicense(dialogInfo.ToModify.Id));
        
        Confirm = new DelegateCommand(modifyAndCloseDialog, () => SelectedProduct != null && !string.IsNullOrWhiteSpace(Key))
            .ObservesProperty(() => SelectedProduct)
            .ObservesProperty(() => Key);
        Abort = new DelegateCommand(() =>
        {
            CloseDialog();
            _dialogInfo.AbortedCallback();
        });

        LookUpContract = new PersonLookUpContract();
    }


    private void modifyAndCloseDialog()
    {
        if (SelectedProduct == null) { return; }

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

        var modifiedLicense = Db.Instance.UpdateLicense(
            _dialogInfo.ToModify.Id, 
            Key, 
            SelectedProduct.Id,
            SelectedUsageType.Id, 
            UsageComment, 
            users);

        CloseDialog();
        _dialogInfo.CompletedCallback(modifiedLicense);
    }
}