namespace Fateblade.Licenzeee.WPF.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }= string.Empty;
    public string Version { get; set; }= string.Empty;
    public string Licenser { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
}