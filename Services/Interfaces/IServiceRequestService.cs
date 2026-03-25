using StoRvStar.Models.Entities;

namespace StoRvStar.Services.Interfaces;

public interface IServiceRequestService
{
    List<ServiceRequest> GetAll();
    ServiceRequest GetById(int id);
    void Create(ServiceRequest request);
}