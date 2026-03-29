using StoRvStar.Data;
using StoRvStar.Models.Entities;
using StoRvStar.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        return _context.ServiceRequests
            .Include(r => r.User)
            .Include(r => r.Car)
            .Include(r => r.ServiceItems)
                .ThenInclude(si => si.Service)
            .ToList();
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
    public void UpdateStatus(int id, string status)
    {
        var request = _context.ServiceRequests.FirstOrDefault(r => r.Id == id);

        if (request != null)
        {
            request.Status = status;
            _context.SaveChanges();
        }
    }

}