using System;

namespace Fateblade.Licenzeee.WPF.Events;

internal class UserYesNoRequest
{
    public string Header { get; set; }
    public string Question { get; set; }
    public Action<bool> UserDecisionCallback { get; set; }

    public UserYesNoRequest(string header, string question, Action<bool> userDecisionCallback)
    {
        Header = header;
        Question = question;
        UserDecisionCallback = userDecisionCallback;
    }
}