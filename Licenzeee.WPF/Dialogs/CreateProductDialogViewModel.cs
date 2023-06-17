using Fateblade.Licenzeee.WPF.Events;
using Fateblade.Licenzeee.WPF.Models;
using Prism.Commands;
using Prism.Events;

namespace Fateblade.Licenzeee.WPF.Dialogs
{
    internal class CreateProductDialogViewModel : DialogBindableBase
    {
        private readonly ShowCreateDialog<Product> _dialogInfo;


        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _version = string.Empty;
        public string Version
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }

        private string _licenser = string.Empty;
        public string Licenser
        {
            get => _licenser;
            set => SetProperty(ref _licenser, value);
        }

        private string _comment = string.Empty;
        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        public DelegateCommand Create { get; }
        public DelegateCommand Abort { get; }


        public CreateProductDialogViewModel(IEventAggregator eventAggregator, ShowCreateDialog<Product> dialogInfo)
            : base(eventAggregator, dialogInfo)
        {
            _dialogInfo = dialogInfo;

            Create = new DelegateCommand(
                    createAndClose,
                    () => !string.IsNullOrWhiteSpace(Name))
                .ObservesProperty(() => Name);

            Abort = new DelegateCommand(close);
        }


        private void createAndClose()
        {
            var createdProduct = Db.Instance.CreateProduct(Name, Version, Licenser, Comment);

            CloseDialog();
            _dialogInfo.CreationCompletedCallback(createdProduct);
        }

        private void close()
        {
            CloseDialog();
            _dialogInfo.CreationAbortedCallback();
        }
    }
}
        