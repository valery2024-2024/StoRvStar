using StoRvStar.Models.Entities;

namespace StoRvStar.Models.ViewModels;

public class CreateServiceRequestVM
{
    public int CarId { get; set; }
    public int UserId { get; set; }

    public List<Service> Services { get; set; } = new();
    public List<Car> Cars { get; set; } = new();

    public List<int> SelectedServices { get; set; } = new();
    public List<User> Users { get; set; } = new();
}