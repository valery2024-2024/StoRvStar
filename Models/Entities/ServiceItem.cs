namespace StoRvStar.Models.Entities;

public class ServiceItem
{
    public int Id { get; set; }

    public int ServiceRequestId { get; set; }
    public ServiceRequest ServiceRequest { get; set; }

    public int ServiceId { get; set; }
    public Service Service { get; set; }

    public int Quantity { get; set; }
    public decimal Price { get; set; }
}