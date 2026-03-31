using StoRvStar.Data;
using StoRvStar.Models.Entities;
using StoRvStar.Models.ViewModels;
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

    public List<User> GetUsers()
    {
        return _context.Users.ToList();
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

    public List<ServiceRequestItemVM> GetAllForIndex()
    {
        var data = _context.ServiceRequests
            .Include(r => r.User)
            .Include(r => r.Car)
            .Include(r => r.ServiceItems)
                .ThenInclude(si => si.Service)
            .ToList();

            return data.Select(r => new ServiceRequestItemVM
            {
                Id = r.Id,
                ClientName = r.User != null ? r.User.Name : "",
                ClientPhone = r.User != null ? r.User.Phone : "",
                CarName = r.Car != null ? $"{r.Car.Brand} {r.Car.Model}" : "",
                PlateNumber = r.Car != null ? r.Car.PlateNumber : "",

                Services = r.ServiceItems
                    .Where(si => si.Service != null)
                    .Select(si => si.Service.Name)
                    .ToList(),

                TotalPrice = r.TotalPrice,
                Status = r.Status,

                StatusText = r.Status switch
                {
                    "New" => "Нова",
                    "InProgress" => "В роботі",
                    "Done" => "Готово",
                    _ => r.Status
                },

                CreatedAt = r.CreatedAt
            }).ToList();
    }

    public ServiceRequest GetById(int id)
    {
        return _context.ServiceRequests
            .Include(r => r.User)
            .Include(r => r.Car)
            .Include(r => r.ServiceItems)
                .ThenInclude(si => si.Service)
            .FirstOrDefault(r => r.Id == id);
    }

    public List<Service> GetServices()
    {
        return _context.Services.ToList();
    }

    public List<Car> GetCars()
    {
        return _context.Cars.ToList();
    }

    public void Create(CreateServiceRequestVM vm)
    {
        var request = new ServiceRequest
        {
            CarId = vm.SelectedCarId,
            UserId = vm.SelectedUserId,
            Description = vm.Description,
            CreatedAt = DateTime.Now,
            Status = "New"
        };

        _context.ServiceRequests.Add(request);
        _context.SaveChanges();

        decimal total = 0;

        foreach (var serviceId in vm.SelectedServiceIds)
        {
            var service = _context.Services.Find(serviceId);

            var price = service?.Price ?? 0;
            total += price;

            var item = new ServiceItem
            {
                ServiceRequestId = request.Id,
                ServiceId = serviceId,
                Price = price
            };

            _context.ServiceItems.Add(item);
        }

        request.TotalPrice = total;

        _context.SaveChanges();
    }

    public CreateServiceRequestVM GetEditVM(int id)
    {
        var request = _context.ServiceRequests
            .Include(r => r.ServiceItems)
            .FirstOrDefault(r => r.Id == id);

        if (request == null) return null;

        return new CreateServiceRequestVM
        {
            SelectedCarId = request.CarId,
            SelectedUserId = request.UserId,
            SelectedServiceIds = request.ServiceItems.Select(s => s.ServiceId).ToList(),

            Cars = _context.Cars.ToList(),
            Services = _context.Services.ToList(),
            Users = _context.Users.ToList()
        };
    }

    public void Update(int id, CreateServiceRequestVM vm)
    {
        var request = _context.ServiceRequests
            .Include(r => r.ServiceItems)
            .FirstOrDefault(r => r.Id == id);

        if (request == null) return;

        request.CarId = vm.SelectedCarId;
        request.UserId = vm.SelectedUserId;
        request.Description = vm.Description;

        _context.ServiceItems.RemoveRange(request.ServiceItems);

        decimal total = 0;

        foreach (var serviceId in vm.SelectedServiceIds)
        {
            var service = _context.Services.Find(serviceId);

            var price = service?.Price ?? 0;
            total += price;

            _context.ServiceItems.Add(new ServiceItem
            {
                ServiceRequestId = request.Id,
                ServiceId = serviceId,
                Price = price
            });
        }

        request.TotalPrice = total;

        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var request = _context.ServiceRequests
            .Include(r => r.ServiceItems)
            .FirstOrDefault(r => r.Id == id);

        if (request == null) return;

        _context.ServiceItems.RemoveRange(request.ServiceItems);
        _context.ServiceRequests.Remove(request);

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