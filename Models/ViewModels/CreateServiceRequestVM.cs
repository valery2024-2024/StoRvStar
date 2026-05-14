using StoRvStar.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace StoRvStar.Models.ViewModels;

public class CreateServiceRequestVM
{
    [Range(1, int.MaxValue, ErrorMessage = "Оберіть клієнта")]
    public int SelectedUserId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Оберіть авто")]
    public int SelectedCarId { get; set; }

    [MinLength(1, ErrorMessage = "Оберіть хоча б одну послугу")]
    public List<int> SelectedServiceIds { get; set; } = new();

    public List<Car> Cars { get; set; } = new();
    public List<Service> Services { get; set; } = new();
    public List<User> Users { get; set; } = new();

    [StringLength(500, ErrorMessage = "Опис не може бути довшим за 500 символів")]
    public string? Description { get; set; }
}
