using Fateblade.Licenzeee.WPF.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using Fateblade.Licenzee.Db;
using Fateblade.Licenzee.Db.Models;
using License = Fateblade.Licenzee.Db.Models.License;
using Fateblade.Licenzeee.WPF.Db;

namespace Fateblade.Licenzeee.WPF.Views
{
    internal class LicensesViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDb _db;

        private bool _isCreatingNew;
        public bool IsCreatingNew
        {
            get => _isCreatingNew;
            set => SetProperty(ref _isCreatingNew, value);
        }

        private ObservableCollection<License> _licenses;
        public ObservableCollection<License> Licenses
        {
            get => _licenses;
            set => SetProperty(ref _licenses, value);
        }
        
        private License? _selectedLicense;
        public License? SelectedLicense
        {
            get => _selectedLicense;
            set => SetProperty(ref _selectedLicense, value);
        }

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }
        
        private Product? _filterProduct;
        public Product? FilterProduct
        {
            get => _filterProduct;
            set => SetProperty(ref _filterProduct, value, filter);
        }
        
        public DelegateCommand AddNew { get; }
        public DelegateCommand DeleteSelected { get; }
        public DelegateCommand ModifySelected { get; }

        public LicensesViewModel(IEventAggregator eventAggregator, IDb db)
        {
            _eventAggregator = eventAggregator;
            _db = db;

            AddNew = new DelegateCommand(
                () =>
                {
                    IsCreatingNew = true;
                    _eventAggregator.GetEvent<PubSubEvent<ShowCreateDialog<License>>>().Publish(new ShowCreateDialog<License>(
                        "Add New License",
                        filterAndSelect,
                        ()=>
                        {
                            IsCreatingNew = false;
                        })); /*Request CreationDialog*/
                },()=>!IsCreatingNew)
                .ObservesProperty(()=> IsCreatingNew);

            ModifySelected = new DelegateCommand(
                () =>
                {
                    IsCreatingNew = true;
                    _eventAggregator.GetEvent<PubSubEvent<ShowModifyDialog<License>>>().Publish(new ShowModifyDialog<License>(
                        "Modify License",
                        SelectedLicense!,
                        filterAndSelect,
                        () =>
                        {
                            IsCreatingNew = false;
                        })); /*Request CreationDialog*/
                },
                () => !IsCreatingNew&& SelectedLicense != null )
                .ObservesProperty(() => IsCreatingNew)
                .ObservesProperty(()=>SelectedLicense);

            DeleteSelected = new DelegateCommand(
                () => _eventAggregator.GetEvent<PubSubEvent<UserConfirmationRequest>>().Publish(
                    new UserConfirmationRequest(
                        "Are you sure?",
                        "This will delete the selected License. This action cannot be undone",
                        handleDeleteUserConfirmation)),
                () => SelectedLicense != null)
                .ObservesProperty(()=>SelectedLicense);

            Products = new ObservableCollection<Product>(_db.Products);

            filter();
        }

        private void filterAndSelect(License toSelect)
        {
            filter();
            SelectedLicense = Licenses.First(t => t.Id == toSelect.Id);
            IsCreatingNew = false;
        }
        
        private void handleDeleteUserConfirmation(bool userConfirmed)
        {
            if (!userConfirmed || SelectedLicense==null) {return;}

            _db.DeleteLicense(SelectedLicense.Id);

            filter();
        }

        private void filter()
        {
            Licenses = FilterProduct != null ? new ObservableCollection<License>(_db.Licenses.Where(t=> t.ProductId == FilterProduct.Id)) : 
                new ObservableCollection<License>(_db.Licenses);
        }


    }
}
