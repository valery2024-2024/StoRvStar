using StoRvStar.Models.Entities;

namespace StoRvStar.Services.Interfaces;

public interface IServiceRequestService
{
    List<ServiceRequest> GetAll();
    ServiceRequest GetById(int id);
    List<Service> GetServices();
    List<Car> GetCars();
    void Create(ServiceRequest request, List<int> selectedServices);
    void UpdateStatus(int id, string status);
}