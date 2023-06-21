using Fateblade.Licenzee.Db.Models;
using Fateblade.Licenzeee.WPF.Events;
using Prism.Commands;
using Prism.Events;

namespace Fateblade.Licenzeee.WPF.Dialogs
{
    internal class CreateProductDialogViewModel : ProductDialogBaseViewModel
    {
        private readonly ShowCreateDialog<Product> _dialogInfo;

        
        public CreateProductDialogViewModel(IEventAggregator eventAggregator, ShowCreateDialog<Product> dialogInfo)
            : base(eventAggregator, dialogInfo)
        {
            _dialogInfo = dialogInfo;

            Confirm = new DelegateCommand(
                    createAndClose,
                    () => !string.IsNullOrWhiteSpace(Name))
                .ObservesProperty(() => Name);

            Abort = new DelegateCommand(close);
        }


        private void createAndClose()
        {
            var createdProduct = InMemoryDb.Instance.CreateProduct(Name, Version, Licenser, Comment);

            CloseDialog();
            _dialogInfo.CompletedCallback(createdProduct);
        }

        private void close()
        {
            CloseDialog();
            _dialogInfo.AbortedCallback();
        }
    }
}
        