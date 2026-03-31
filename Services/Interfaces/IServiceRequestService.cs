using StoRvStar.Models.Entities;
using StoRvStar.Models.ViewModels;

namespace StoRvStar.Services.Interfaces;

public interface IServiceRequestService
{
    List<User> GetUsers();
    List<ServiceRequest> GetAll();
    List<ServiceRequestItemVM> GetAllForIndex();
    ServiceRequest GetById(int id);
    List<Service> GetServices();
    List<Car> GetCars();

    void Create(CreateServiceRequestVM vm);

    CreateServiceRequestVM GetEditVM(int id);
    void Update(int id, CreateServiceRequestVM vm);
    void Delete(int id);

    void UpdateStatus(int id, string status);
}