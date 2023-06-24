using Fateblade.Licenzeee.WPF.Events;
using Fateblade.Licenzeee.WPF.Inputs;
using Fateblade.Licenzeee.WPF.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using Fateblade.Licenzee.Db.Models;
using Fateblade.Licenzeee.WPF.Dialogs;
using Fateblade.Licenzee.Db;
using Fateblade.Licenzeee.WPF.Db;

namespace Fateblade.Licenzeee.WPF
{
    internal class MainWindowViewModel : BindableBase
    {
        private readonly Stack<BindableBaseWithHeader> _dialogStack;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDbProvider _dbProvider;
        private readonly DbImportHandler _dbImportHandler;


        private BindableBase? _displayedContent;
        public BindableBase? DisplayedContent
        {
            get => _displayedContent;
            set => SetProperty(ref _displayedContent, value);
        }

        private BindableBaseWithHeader? _displayedInputRequest;
        public BindableBaseWithHeader? DisplayedInputRequest
        {
            get => _displayedInputRequest;
            set => SetProperty(ref _displayedInputRequest, value);
        }

        private BindableBaseWithHeader? _displayedDialog;
        public BindableBaseWithHeader? DisplayedDialog
        {
            get => _displayedDialog;
            set => SetProperty(ref _displayedDialog, value);
        }

        public DelegateCommand SwitchToLicenses { get; }
        public DelegateCommand SwitchToProducts { get; }
        public DelegateCommand SwitchToUsers { get; }
        public DelegateCommand SwitchToOptions { get; }


        public MainWindowViewModel(IEventAggregator eventAggregator, IDbProvider dbProvider)
        {
            _eventAggregator = eventAggregator;
            _dbProvider = dbProvider;
            _dialogStack = new Stack<BindableBaseWithHeader>();
            _dbImportHandler = new DbImportHandler(_eventAggregator, dbProvider);

            SwitchToLicenses = new DelegateCommand(() => DisplayedContent = new LicensesViewModel(_eventAggregator, _dbProvider.Db));
            SwitchToProducts = new DelegateCommand(() => DisplayedContent = new ProductsViewModel(_eventAggregator, _dbProvider.Db));
            SwitchToUsers = new DelegateCommand(() => DisplayedContent = new UsersViewModel(_eventAggregator, _dbProvider.Db));
            SwitchToOptions = new DelegateCommand(() => DisplayedContent = new OptionsViewModel(_eventAggregator));

            _eventAggregator.GetEvent<PubSubEvent<UserConfirmationRequest>>().Subscribe(handleUserConfirmationRequest);
            _eventAggregator.GetEvent<PubSubEvent<UserYesNoRequest>>().Subscribe(handleYesNoRequest);
            _eventAggregator.GetEvent<PubSubEvent<UserInfoRequest>>().Subscribe(handleUserInfoRequest);
            _eventAggregator.GetEvent<PubSubEvent<CloseCurrentInputRequest>>().Subscribe(handleCLoseCurrentInputRequest);
            _eventAggregator.GetEvent<PubSubEvent<CloseCurrentDialogRequest>>().Subscribe(handleCloseCurrentDialogRequest);
            _eventAggregator.GetEvent<PubSubEvent<ShowCreateDialog<License>>>().Subscribe(handleShowCreateLicenseDialog);
            _eventAggregator.GetEvent<PubSubEvent<ShowCreateDialog<Product>>>().Subscribe(handleCreateProductDialog);
            _eventAggregator.GetEvent<PubSubEvent<ShowCreateDialog<User>>>().Subscribe(handleCreateUserDialog);
            _eventAggregator.GetEvent<PubSubEvent<ShowModifyDialog<License>>>().Subscribe(handleModifyLicenseDialog);
            _eventAggregator.GetEvent<PubSubEvent<ShowModifyDialog<Product>>>().Subscribe(handleModifyProductDialog);
            _eventAggregator.GetEvent<PubSubEvent<ShowModifyDialog<User>>>().Subscribe(handleModifyUserDialog);
        }


        private void handleUserConfirmationRequest(UserConfirmationRequest obj)
        {
            DisplayedInputRequest = new UserConfirmationInputViewModel(obj, _eventAggregator);
        }

        private void handleYesNoRequest(UserYesNoRequest obj)
        {
            DisplayedInputRequest = new UserYesNoInputViewModel(obj, _eventAggregator);
        }

        private void handleUserInfoRequest(UserInfoRequest obj)
        {
            DisplayedInputRequest = new UserTextInfoViewModel(obj, _eventAggregator);
        }

        private void handleCLoseCurrentInputRequest(CloseCurrentInputRequest obj)
        {
            DisplayedInputRequest = null;
        }

        private void handleCloseCurrentDialogRequest(CloseCurrentDialogRequest obj)
        {
            DisplayedDialog = null;

            popPreviousDialogFromStack();
        }

        private void handleShowCreateLicenseDialog(ShowCreateDialog<License> obj)
        {
            pushCurrentDialogToStack();

            DisplayedDialog = new CreateLicenseDialogViewModel(_eventAggregator, obj, _dbProvider.Db);
        }
        
        private void handleCreateProductDialog(ShowCreateDialog<Product> obj)
        {
            pushCurrentDialogToStack();

            DisplayedDialog = new CreateProductDialogViewModel(_eventAggregator, obj, _dbProvider.Db);
        }

        private void handleCreateUserDialog(ShowCreateDialog<User> obj)
        {
            pushCurrentDialogToStack();

            DisplayedDialog = new CreateUserDialogViewModel(_eventAggregator, obj, _dbProvider.Db);
        }

        private void handleModifyLicenseDialog(ShowModifyDialog<License> obj)
        {
            pushCurrentDialogToStack();

            DisplayedDialog = new ModifyLicenseDialogViewModel(_eventAggregator, obj, _dbProvider.Db);
        }

        private void handleModifyProductDialog(ShowModifyDialog<Product> obj)
        {
            pushCurrentDialogToStack();

            DisplayedDialog = new ModifyProductDialogViewModel(_eventAggregator, obj, _dbProvider.Db);
        }

        private void handleModifyUserDialog(ShowModifyDialog<User> obj)
        {
            pushCurrentDialogToStack();

            DisplayedDialog = new ModifyUserDialogViewModel(_eventAggregator, obj, _dbProvider.Db);
        }

        private void pushCurrentDialogToStack()
        {
            if (DisplayedDialog == null) { return; }

            _dialogStack.Push(DisplayedDialog);
        }

        private void popPreviousDialogFromStack()
        {
            if(_dialogStack.Count == 0) { return; }

            DisplayedDialog = _dialogStack.Pop();
        }
    }
}
