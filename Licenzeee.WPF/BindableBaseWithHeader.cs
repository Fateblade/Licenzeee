using Fateblade.Licenzeee.WPF.Events;
using Prism.Commands;
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

        protected DialogBindableBase(IEventAggregator eventAggregator,ShowDialogBase dialogInfo)
        {
            EventAggregator = eventAggregator;
            Header = dialogInfo.Header;
        }

        protected void CloseDialog()
        {
            EventAggregator.GetEvent<PubSubEvent<CloseCurrentDialogRequest>>().Publish(new CloseCurrentDialogRequest());
        }
    }

    internal abstract class ConfirmableDialogBindableBase : DialogBindableBase
    {
        public DelegateCommand Confirm { get; protected set; }
        public DelegateCommand Abort { get;protected set; }


        protected ConfirmableDialogBindableBase(IEventAggregator eventAggregator, ShowDialogBase dialogInfo) 
            : base(eventAggregator, dialogInfo)
        {
            Confirm = new DelegateCommand(CloseDialog);
            Abort = new DelegateCommand(CloseDialog);
        }
    }
}
