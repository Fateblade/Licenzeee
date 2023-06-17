using Fateblade.Licenzeee.WPF.Events;
using Fateblade.Licenzeee.WPF.Models;
using Prism.Commands;
using Prism.Events;

namespace Fateblade.Licenzeee.WPF.Dialogs
{
    class CreateUserDialogViewModel : UserDialogBaseViewModel
    {
        public CreateUserDialogViewModel(IEventAggregator eventAggregator, ShowCreateDialog<User> dialogInfo) 
            : base(eventAggregator, dialogInfo)
        {
            Confirm = new DelegateCommand(
                    () =>
                    {
                        var createdUser = Db.Instance.CreateUser(Name, Comment);

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
