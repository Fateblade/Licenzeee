﻿using Fateblade.Licenzee.Db;
using Fateblade.Licenzee.Db.Models;
using Fateblade.Licenzeee.WPF.Events;
using Prism.Commands;
using Prism.Events;

namespace Fateblade.Licenzeee.WPF.Dialogs
{
    class CreateUserDialogViewModel : UserDialogBaseViewModel
    {
        public CreateUserDialogViewModel(IEventAggregator eventAggregator, ShowCreateDialog<User> dialogInfo, IDb db) 
            : base(eventAggregator, dialogInfo, db)
        {
            Confirm = new DelegateCommand(
                    () =>
                    {
                        var createdUser = Db.CreateUser(Name, Comment);

                        CloseDialog();
                        dialogInfo.CompletedCallback(createdUser);
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
}
