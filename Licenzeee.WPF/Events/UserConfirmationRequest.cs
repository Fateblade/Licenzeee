using System;

namespace Fateblade.Licenzeee.WPF.Events
{
    internal class UserConfirmationRequest
    {
        public string Header { get; set; }
        public string Question { get; set; }
        public Action<bool> UserConfirmationCallback { get; set; }

        public UserConfirmationRequest(string header, string question, Action<bool> userConfirmationCallback)
        {
            Header = header;
            Question = question;
            UserConfirmationCallback = userConfirmationCallback;
        }
    }
}
