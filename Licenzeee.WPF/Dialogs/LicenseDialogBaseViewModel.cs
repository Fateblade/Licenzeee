using System.Collections.ObjectModel;
using System.Linq;
using BlackPearl.Controls.Contract;
using Fateblade.Licenzeee.WPF.Events;
using Fateblade.Licenzeee.WPF.Models;
using Prism.Commands;
using Prism.Events;

namespace Fateblade.Licenzeee.WPF.Dialogs;

internal abstract class LicenseDialogBaseViewModel : DialogBindableBase
{
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

    private string _key = string.Empty;
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

    private string _usageComment = string.Empty;
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

    public ILookUpContract LookUpContract { get; protected set; }

    public DelegateCommand Confirm { get; protected set; }
    public DelegateCommand Abort { get; protected set; }


    protected LicenseDialogBaseViewModel(IEventAggregator eventAggregator, ShowDialogBase dialogInfo) : base(eventAggregator, dialogInfo)
    {
        Products = new ObservableCollection<Product>(Db.Instance.Products);
        UsageTypes = new ObservableCollection<UsageType>(Db.Instance.UsageTypes);
        SelectedUsageType = UsageTypes.First();

        Users = new ObservableCollection<User>(Db.Instance.Users);

    }
}