namespace StoRvStar.Models.Entities;

public class Service
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    // Navigation
    public List<ServiceItem> ServiceItems { get; set; }
}