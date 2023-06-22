using Fateblade.Licenzee.Db;
using Fateblade.Licenzeee.WPF.Events;
using Prism.Events;

namespace Fateblade.Licenzeee.WPF.Dialogs;

internal class ProductDialogBaseViewModel : ConfirmableDialogBindableBase
{
    protected IDb Db { get; }


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


    public ProductDialogBaseViewModel(IEventAggregator eventAggregator, ShowDialogBase dialogInfo, IDb db)
        : base(eventAggregator, dialogInfo)
    {
        Db = db;
    }
}