using StoRvStar.Models.Entities;

namespace StoRvStar.Models.ViewModels;

public class CreateServiceRequestVM
{
    public int CarId { get; set; }
    public int UserId { get; set; }

    public List<Service> Services { get; set; }
    public List<Car> Cars { get; set; }

    public List<int> SelectedServices { get; set; }
}