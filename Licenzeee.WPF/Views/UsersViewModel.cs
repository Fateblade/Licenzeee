using System.Collections.ObjectModel;
using System.Linq;
using Fateblade.Licenzeee.WPF.Events;
using Fateblade.Licenzeee.WPF.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Fateblade.Licenzeee.WPF.Views
{
    internal class UsersViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        
        private bool _isCreating;
        public bool IsCreating
        {
            get => _isCreating;
            set => SetProperty(ref _isCreating, value);
        }

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        private User? _selectedUser;
        public User? SelectedUser
        {
            get => _selectedUser;
            set => SetProperty(ref _selectedUser, value);
        }

        private string _filterText;
        public string FilterText
        {
            get => _filterText;
            set => SetProperty(ref _filterText, value);
        }
        
        public DelegateCommand AddNew { get; }
        public DelegateCommand DeleteSelected { get; }
        public DelegateCommand ModifySelected { get; }

        public UsersViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            AddNew = new DelegateCommand(
                    () =>
                    {
                        IsCreating = true;
                        _eventAggregator.GetEvent<PubSubEvent<ShowCreateDialog<User>>>().Publish(
                            new ShowCreateDialog<User>(
                                "Add a new license user",
                                handleCreationCompleted,
                                () => IsCreating = false
                            ));
                    },
                    () => !IsCreating)
                .ObservesProperty(() => IsCreating);

            DeleteSelected = new DelegateCommand(
                    ()=>
                    {
                        _eventAggregator.GetEvent<PubSubEvent<UserConfirmationRequest>>()
                            .Publish(new UserConfirmationRequest(
                                "Delete User?",
                                "This will delete the selected user and remove it from all assigned products. This action cannot be undone!",
                                deleteConfirmation));
                    },
                    () => SelectedUser != null)
                .ObservesProperty(() => SelectedUser);

            filter();
        }


        private void deleteConfirmation(bool userConfirmed)
        {
            if (!userConfirmed || SelectedUser==null) return;

            Db.Instance.DeleteUser(SelectedUser.Id);
            SelectedUser = null;

            filter();
        }

        private void handleCreationCompleted(User createdUser)
        {
            filter();

            SelectedUser = Users.First(t => t.Id == createdUser.Id);
        }

        private void filter()
        {
            if (!string.IsNullOrWhiteSpace(FilterText))
            {
                Users = new ObservableCollection<User>(
                    Db.Instance.Users.Where(
                        t => t.Name.ToLower().Contains(FilterText.ToLower())
                             || t.Comment.ToLower().Contains(FilterText.ToLower())));
            }
            else
            {
                Users = new ObservableCollection<User>(Db.Instance.Users);
            }
        }
    }
}
