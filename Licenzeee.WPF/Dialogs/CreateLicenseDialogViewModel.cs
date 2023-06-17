using System;
using System.Collections.ObjectModel;
using System.Linq;
using BlackPearl.Controls.Contract;
using Fateblade.Licenzeee.WPF.Events;
using Fateblade.Licenzeee.WPF.LookUpContracts;
using Fateblade.Licenzeee.WPF.Models;
using Prism.Commands;
using Prism.Events;

namespace Fateblade.Licenzeee.WPF.Dialogs
{
    internal class CreateLicenseDialogViewModel : DialogBindableBase
    {
        private readonly ShowCreateDialog<License> _dialogInfo;

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        private Product? _selectedProduct;
        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        private ObservableCollection<UsageType> _usageTypes;
        public ObservableCollection<UsageType> UsageTypes
        {
            get => _usageTypes;
            set => SetProperty(ref _usageTypes, value);
        }

        private UsageType _selectedUsageType;
        public UsageType SelectedUsageType
        {
            get => _selectedUsageType;
            set => SetProperty(ref _selectedUsageType, value);
        }

        private string _key=string.Empty;
        public string Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
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

        private string _usageComment=string.Empty;
        public string UsageComment
        {
            get => _usageComment;
            set => SetProperty(ref _usageComment, value);
        }

        private ObservableCollection<User> _selectedUsers = new();
        public ObservableCollection<User> SelectedUsers
        {
            get => _selectedUsers;
            set => SetProperty(ref _selectedUsers, value);
        }

        public ILookUpContract LookUpContract { get; }

        public DelegateCommand Create { get; }
        public DelegateCommand Abort { get; }


        public CreateLicenseDialogViewModel(IEventAggregator eventAggregator, ShowCreateDialog<License> dialogInfo) : base(eventAggregator, dialogInfo)
        {
            _dialogInfo = dialogInfo;

            Products = new ObservableCollection<Product>(Db.Instance.Products);
            UsageTypes = new ObservableCollection<UsageType>(Db.Instance.UsageTypes);
            SelectedUsageType = UsageTypes.First();

            Users = new ObservableCollection<User>(Db.Instance.Users);

            Create = new DelegateCommand(createAndCloseDialog, ()=>SelectedProduct!=null&&!string.IsNullOrWhiteSpace(Key))
                .ObservesProperty(()=>SelectedProduct)
                .ObservesProperty(() => Key);
            Abort = new DelegateCommand(()=>
            {
                CloseDialog();
                _dialogInfo.CreationAbortedCallback();
            });

            LookUpContract = new PersonLookUpContract();
        }


        private void createAndCloseDialog()
        {
            if (SelectedProduct == null) return;

            User[] users;
            if (SelectedUsageType.Id == 2)
            {
                users = SelectedUser != null ? new[] { SelectedUser } : users = Array.Empty<User>();
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
            _dialogInfo.CreationCompletedCallback(createdLicense);
        }
    }
}
