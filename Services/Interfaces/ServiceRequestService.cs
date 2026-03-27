using StoRvStar.Data;
using StoRvStar.Models.Entities;
using StoRvStar.Services.Interfaces;

namespace StoRvStar.Services;

public class ServiceRequestService : IServiceRequestService
{
    private readonly AppDbContext _context;

    public ServiceRequestService(AppDbContext context)
    {
        _context = context;
    }

    public List<ServiceRequest> GetAll()
    {
        return _context.ServiceRequests.ToList();
    }

    public ServiceRequest GetById(int id)
    {
        return _context.ServiceRequests.FirstOrDefault(x => x.Id == id);
    }

    public List<Service> GetServices()
    {
        return _context.Services.ToList();
    }

    public List<Car> GetCars()
    {
        return _context.Cars.ToList();
    }

    public void Create(ServiceRequest request, List<int> selectedServices)
    {
        request.CreatedAt = DateTime.Now;
        request.Status = "New";

        _context.ServiceRequests.Add(request);
        _context.SaveChanges();

        decimal total = 0;

        foreach (var serviceId in selectedServices)
        {
            var service = _context.Services.FirstOrDefault(s => s.Id == serviceId);

            if (service != null)total += service.Price;

            var item = new ServiceItem
            {
                ServiceRequestId = request.Id,
                ServiceId = serviceId
            };

            _context.ServiceItems.Add(item);
        }
        request.TotalPrice = total;

        _context.SaveChanges();
    }
}