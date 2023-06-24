using System;

namespace Fateblade.Licenzeee.WPF.Events;

internal class UserInfoRequest
{
    public string Header { get; set; }
    public string Info { get; set; }
    public Action? UserClosedInfoCallback { get; set; }

    public UserInfoRequest(string header, string info, Action? userClosedInfoCallback = null)
    {
        Header = header;
        Info = info;
        UserClosedInfoCallback = userClosedInfoCallback;
    }
}