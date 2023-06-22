using System.Collections.ObjectModel;
using System.Linq;
using Fateblade.Licenzee.Db;
using Fateblade.Licenzee.Db.Models;
using Fateblade.Licenzeee.WPF.Db;
using Fateblade.Licenzeee.WPF.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Fateblade.Licenzeee.WPF.Views
{
    internal class UsersViewModel : BindableBase
    {
        private readonly IDb _db;
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

        public UsersViewModel(IEventAggregator eventAggregator, IDb db)
        {
            _db = db;
            var eventAggregator1 = eventAggregator;

            AddNew = new DelegateCommand(
                    () =>
                    {
                        IsCreating = true;
                        eventAggregator1.GetEvent<PubSubEvent<ShowCreateDialog<User>>>().Publish(
                            new ShowCreateDialog<User>(
                                "Add a new license user",
                                filterAndSelect,
                                () => IsCreating = false
                            ));
                    },
                    () => !IsCreating)
                .ObservesProperty(() => IsCreating);

            ModifySelected = new DelegateCommand(
                () =>
                {
                    IsCreating = true;
                    eventAggregator1.GetEvent<PubSubEvent<ShowModifyDialog<User>>>().Publish(new ShowModifyDialog<User>(
                        "Modify User",
                        SelectedUser!,
                        filterAndSelect,
                        () =>
                        {
                            IsCreating = false;
                        })); /*Request CreationDialog*/
                },
                () => !IsCreating && SelectedUser != null)
                .ObservesProperty(()=>IsCreating)
                .ObservesProperty(() => SelectedUser);

            DeleteSelected = new DelegateCommand(
                    ()=>
                    {
                        eventAggregator1.GetEvent<PubSubEvent<UserConfirmationRequest>>()
                            .Publish(new UserConfirmationRequest(
                                "Delete User?",
                                "This will delete the selected user and remove it from all assigned products. This action cannot be undone!",
                                deleteConfirmation));
                    },
                    () => SelectedUser != null)
                .ObservesProperty(() => IsCreating)
                .ObservesProperty(() => SelectedUser);

            filter();
        }


        private void deleteConfirmation(bool userConfirmed)
        {
            if (!userConfirmed || SelectedUser==null) { return; }

            _db.DeleteUser(SelectedUser.Id);
            SelectedUser = null;

            filter();
        }

        private void filterAndSelect(User createdUser)
        {
            filter();

            SelectedUser = Users.First(t => t.Id == createdUser.Id);
        }

        private void filter()
        {
            if (!string.IsNullOrWhiteSpace(FilterText))
            {
                Users = new ObservableCollection<User>(
                    _db.Users.Where(
                        t => t.Name.ToLower().Contains(FilterText.ToLower())
                             || t.Comment.ToLower().Contains(FilterText.ToLower())));
            }
            else
            {
                Users = new ObservableCollection<User>(_db.Users);
            }
        }
    }
}
