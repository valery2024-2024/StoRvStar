using System.ComponentModel.DataAnnotations;

namespace StoRvStar.Models.Entities;

public class Car
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Вкажіть марку")]
    [StringLength(50, ErrorMessage = "Марка не може бути довшою за 50 символів")]
    public string Brand { get; set; }

    [Required(ErrorMessage = "Вкажіть модель")]
    [StringLength(50, ErrorMessage = "Модель не може бути довшою за 50 символів")]
    public string Model { get; set; }

    [Range(1900, 2100, ErrorMessage = "Некоректний рік авто")]
    public int Year { get; set; }

    [Required(ErrorMessage = "Вкажіть номер авто")]
    [StringLength(20, ErrorMessage = "Номер авто не може бути довшим за 20 символів")]
    public string PlateNumber { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Оберіть клієнта")]
    public int UserId { get; set; }
    public User User { get; set; }

    // Navigation
    public List<ServiceRequest> ServiceRequests { get; set; } = new();
}
