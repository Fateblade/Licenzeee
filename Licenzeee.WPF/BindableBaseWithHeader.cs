using Fateblade.Licenzeee.WPF.Events;
using Prism.Events;
using Prism.Mvvm;

namespace Fateblade.Licenzeee.WPF
{
    internal abstract class BindableBaseWithHeader : BindableBase
    {
        private string _Header;
        public string Header
        {
            get => _Header;
            set => SetProperty(ref _Header, value);
        }
    }

    internal abstract class DialogBindableBase : BindableBaseWithHeader
    {
        protected IEventAggregator EventAggregator { get; }

        protected DialogBindableBase(IEventAggregator eventAggregator,ShowDialog dialogInfo)
        {
            EventAggregator = eventAggregator;
            Header = dialogInfo.Header;
        }

        protected void CloseDialog()
        {
            EventAggregator.GetEvent<PubSubEvent<CloseCurrentDialogRequest>>().Publish(new CloseCurrentDialogRequest());
        }
    }
}
