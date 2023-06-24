using System;
using Fateblade.Licenzeee.WPF.Events;
using Prism.Commands;
using Prism.Events;

namespace Fateblade.Licenzeee.WPF.Inputs;

internal class UserYesNoInputViewModel : BindableBaseWithHeader
{
    private readonly IEventAggregator _eventAggregator;
    private readonly Action<bool> _userDecisionCallback;

    private string _question = string.Empty;
    public string Question
    {
        get => _question;
        set => SetProperty(ref _question, value);
    }

    public DelegateCommand Confirm { get; }
    public DelegateCommand Deny { get; }

    public UserYesNoInputViewModel(UserYesNoRequest dialogIssuer, IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
        Header = dialogIssuer.Header;
        Question = dialogIssuer.Question;

        _userDecisionCallback = dialogIssuer.UserDecisionCallback;

        Confirm = new DelegateCommand(() => closeInputRequest(true));
        Deny = new DelegateCommand(() => closeInputRequest(false));
    }

    private void closeInputRequest(bool userConfirmed)
    {
        _eventAggregator.GetEvent<PubSubEvent<CloseCurrentInputRequest>>().Publish(new CloseCurrentInputRequest());
        _userDecisionCallback(userConfirmed);
    }
}