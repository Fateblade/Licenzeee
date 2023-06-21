namespace Fateblade.Licenzee.Db.Models;

public class XLicenseUser
{
    public int LicenseId { get; }
    public int UserId { get; }

    public XLicenseUser(int licenseId, int userId)
    {
        LicenseId = licenseId;
        UserId = userId;
    }
}