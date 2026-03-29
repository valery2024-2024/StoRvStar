using StoRvStar.Models.Entities;

namespace StoRvStar.Models.ViewModels;

public class CreateServiceRequestVM
{
    public int SelectedCarId { get; set; }
    public List<int> SelectedServiceIds { get; set; } = new();
    public List<Car> Cars { get; set; } = new();
    public List<Service> Services { get; set; } = new();
    public List<User> Users { get; set; } = new();
    public int SelectedUserId { get; set; }

    public string? Description { get; set; }
}