using System.Collections.ObjectModel;
using System.Linq;
using Fateblade.Licenzee.Db.Models;
using Fateblade.Licenzeee.WPF.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Fateblade.Licenzeee.WPF.Views
{
    internal class ProductsViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        private bool _isCreatingNew;
        public bool IsCreatingNew
        {
            get => _isCreatingNew;
            set => SetProperty(ref _isCreatingNew, value);
        }

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

        private string _filterText;
        public string FilterText
        {
            get => _filterText;
            set => SetProperty(ref _filterText, value, filter);
        }

        public DelegateCommand AddNew { get; }
        public DelegateCommand DeleteSelected { get; }
        public DelegateCommand ModifySelected { get; }

        public ProductsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            AddNew = new DelegateCommand(
                    () =>
                    {
                        IsCreatingNew = true;
                        _eventAggregator.GetEvent<PubSubEvent<ShowCreateDialog<Product>>>().Publish(
                            new ShowCreateDialog<Product>(
                                "Add a new licensed product",
                                filterAndSelect,
                                () => IsCreatingNew = false
                            ));

                    }, 
                () => !IsCreatingNew)
                .ObservesProperty(()=> IsCreatingNew);

            ModifySelected = new DelegateCommand(
                () =>
                {
                    IsCreatingNew = true;
                    _eventAggregator.GetEvent<PubSubEvent<ShowModifyDialog<Product>>>().Publish(new ShowModifyDialog<Product>(
                        "Modify Product",
                        SelectedProduct!,
                        filterAndSelect,
                        () =>
                        {
                            IsCreatingNew = false;
                        })); /*Request CreationDialog*/
                },
                () => !IsCreatingNew && SelectedProduct != null)
                .ObservesProperty(() => IsCreatingNew)
                .ObservesProperty(() => SelectedProduct);

            DeleteSelected = new DelegateCommand(
                    () =>
                    {
                        _eventAggregator.GetEvent<PubSubEvent<UserConfirmationRequest>>().Publish(
                            new UserConfirmationRequest(
                                "Are you sure?",
                                "This will delete the selected Product and all associated Licenses. This action cannot be undone",
                                handleDeleteUserConfirmation));
                    },
                    () => SelectedProduct != null)
                .ObservesProperty(() => SelectedProduct);

            filter();
        }

        private void filterAndSelect(Product obj)
        {
            filter();

            SelectedProduct = Products.FirstOrDefault(t => t.Id == obj.Id);
            IsCreatingNew = false;
        }

        private void handleDeleteUserConfirmation(bool deleteConfirmed)
        {
            if (!deleteConfirmed || SelectedProduct==null) {return;}

            Db.Instance.DeleteProduct(SelectedProduct.Id);

            filter();
        }

        private void filter()
        {
            if (!string.IsNullOrEmpty(FilterText))
            {
                Products = new ObservableCollection<Product>(
                    Db.Instance.Products.Where(
                        t=>
                            t.Name.ToLower().Contains(FilterText.ToLower())
                            || t.Version.ToLower().Contains(FilterText.ToLower())
                            || t.Licenser.ToLower().Contains(FilterText.ToLower())
                            || t.Comment.ToLower().Contains(FilterText.ToLower())));
            }
            else
            {
                Products = new ObservableCollection<Product>(Db.Instance.Products);
            }
        }
    }
}
