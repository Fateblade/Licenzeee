﻿namespace Fateblade.Licenzeee.WPF.Models;

public struct XLicenseUser
{
    public int LicenseId { get; }
    public int UserId { get; }

    public XLicenseUser(int licenseId, int userId)
    {
        LicenseId = licenseId;
        UserId = userId;
    }
}