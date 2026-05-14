using System.ComponentModel.DataAnnotations;

namespace StoRvStar.Models.Entities;

public class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Вкажіть ім'я")]
    [StringLength(100, ErrorMessage = "Ім'я не може бути довшим за 100 символів")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Вкажіть телефон")]
    [Phone(ErrorMessage = "Некоректний номер телефону")]
    [StringLength(30, ErrorMessage = "Телефон не може бути довшим за 30 символів")]
    public string Phone { get; set; }

    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }

    public string? Role { get; set; } // Admin / User

    // Navigation
    public List<Car> Cars { get; set; } = new();
    public List<ServiceRequest> ServiceRequests { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
}
