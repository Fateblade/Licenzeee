namespace Fateblade.Licenzee.Db.Models;

public class License
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public int UsageTypeId { get; set; }
    public string? UsageComment { get; set; }
    public int LicenseUserId { get; set; }
}
