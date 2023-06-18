﻿using Fateblade.Licenzeee.WPF.Events;
using Fateblade.Licenzeee.WPF.Models;
using Prism.Commands;
using Prism.Events;

namespace Fateblade.Licenzeee.WPF.Dialogs;

class ModifyUserDialogViewModel : UserDialogBaseViewModel
{
    public ModifyUserDialogViewModel(IEventAggregator eventAggregator, ShowModifyDialog<User> dialogInfo)
        : base(eventAggregator, dialogInfo)
    {
        Comment = dialogInfo.ToModify.Comment;
        Name = dialogInfo.ToModify.Name;

        Confirm = new DelegateCommand(
                () =>
                {
                    var modifiedUser = Db.Instance.UpdateUser(dialogInfo.ToModify.Id, Name, Comment);

                    CloseDialog();
                    dialogInfo.CompletedCallback(modifiedUser);
                },
                () => !string.IsNullOrWhiteSpace(Name))
            .ObservesProperty(() => Name);

        Abort = new DelegateCommand(() =>
        {
            CloseDialog();
            dialogInfo.AbortedCallback();
        });
    }
}