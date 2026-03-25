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

    public void Create(ServiceRequest request)
    {
        request.CreatedAt = DateTime.Now;
        request.Status = "New";

        _context.ServiceRequests.Add(request);
        _context.SaveChanges();
    }
}