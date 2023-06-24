using System;
using Fateblade.Licenzeee.WPF.Events;
using Prism.Commands;
using Prism.Events;

namespace Fateblade.Licenzeee.WPF.Inputs;

internal class UserTextInfoViewModel : BindableBaseWithHeader
{
    private readonly IEventAggregator _eventAggregator;
    private readonly Action? _userClosedTextInfoCallback;

    private string _info = string.Empty;
    public string Info
    {
        get => _info;
        set => SetProperty(ref _info, value);
    }

    public DelegateCommand Close { get; }

    public UserTextInfoViewModel(UserInfoRequest dialogIssuer, IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
        Header = dialogIssuer.Header;
        Info = dialogIssuer.Info;

        _userClosedTextInfoCallback = dialogIssuer.UserClosedInfoCallback;

        Close = new DelegateCommand(closeRequest);
    }

    private void closeRequest()
    {
        _eventAggregator.GetEvent<PubSubEvent<CloseCurrentInputRequest>>().Publish(new CloseCurrentInputRequest());
        _userClosedTextInfoCallback?.Invoke();
    }
}