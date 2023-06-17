using Fateblade.Licenzeee.WPF.Events;
using Prism.Events;

namespace Fateblade.Licenzeee.WPF.Dialogs;

class UserDialogBaseViewModel : ConfirmableDialogBindableBase
{
    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private string _comment = string.Empty;
    public string Comment
    {
        get => _comment;
        set => SetProperty(ref _comment, value);
    }

    protected UserDialogBaseViewModel(IEventAggregator eventAggregator, ShowDialogBase dialogInfo)
        : base(eventAggregator, dialogInfo)
    {
            
    }
}