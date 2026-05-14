using StoRvStar.Models.Enums;

namespace StoRvStar.Models.Entities;

public class ServiceRequest
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public ServiceRequestStatus Status { get; set; } = ServiceRequestStatus.New;
    public string? Description { get; set; }

    public int CarId { get; set; }
    public Car Car { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
    public decimal TotalPrice { get; set; }

    // Navigation
    public List<ServiceItem> ServiceItems { get; set; } = new();
}
