namespace StoRvStar.Models.Entities;

public class User
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string Phone { get; set; }

    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }

    public string? Role { get; set; } // Admin / User

    // Navigation
    public List<Car> Cars { get; set; }
    public List<ServiceRequest> ServiceRequests { get; set; }
    public List<Review> Reviews { get; set; }
}