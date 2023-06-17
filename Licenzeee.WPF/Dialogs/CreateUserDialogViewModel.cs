using Fateblade.Licenzeee.WPF.Events;
using Fateblade.Licenzeee.WPF.Models;
using Prism.Commands;
using Prism.Events;

namespace Fateblade.Licenzeee.WPF.Dialogs
{
    class CreateUserDialogViewModel : DialogBindableBase
    {
        private readonly ShowCreateDialog<User> _dialogInfo;


        private string _Name;
        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        private string _Comment;
        public string Comment
        {
            get => _Comment;
            set => SetProperty(ref _Comment, value);
        }

        public DelegateCommand Create { get; }
        public DelegateCommand Abort { get; }


        public CreateUserDialogViewModel(IEventAggregator eventAggregator, ShowCreateDialog<User> dialogInfo) 
            : base(eventAggregator, dialogInfo)
        {
            _dialogInfo = dialogInfo;

            Create = new DelegateCommand(
                    () =>
                    {
                        var createdUser = Db.Instance.CreateUser(Name, Comment);

                        CloseDialog();
                        _dialogInfo.CreationCompletedCallback(createdUser);
                    },
                    () => !string.IsNullOrWhiteSpace(_Name))
                .ObservesProperty(() => Name);

            Abort = new DelegateCommand(() =>
            {
                CloseDialog();
                _dialogInfo.CreationAbortedCallback();
            });
        }
    }
}
