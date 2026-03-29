namespace StoRvStar.Models.Entities;

public class ServiceRequest
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }
    public string? Description { get; set; } // New / InProgress / Completed

    public int CarId { get; set; }
    public Car Car { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
    public decimal TotalPrice { get; set; }

    // Navigation
    public List<ServiceItem> ServiceItems { get; set; } = new();
}