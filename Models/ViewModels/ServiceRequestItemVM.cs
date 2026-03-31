namespace StoRvStar.Models.ViewModels;

public class ServiceRequestItemVM
{
    public int Id { get; set; }

    public string ClientName { get; set; } = "";
    public string ClientPhone { get; set; } = "";

    public string CarName { get; set; } = "";
    public string PlateNumber { get; set; } = "";

    public List<string> Services { get; set; } = new();

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = "";

    public string StatusText { get; set; } = "";

    public DateTime CreatedAt { get; set; }
}