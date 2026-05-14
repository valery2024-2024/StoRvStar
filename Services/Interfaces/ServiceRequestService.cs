using Microsoft.EntityFrameworkCore;
using StoRvStar.Data;
using StoRvStar.Models.Entities;
using StoRvStar.Models.Enums;
using StoRvStar.Models.ViewModels;
using StoRvStar.Services.Interfaces;

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
            ClientName = r.User?.Name ?? "",
            ClientPhone = r.User?.Phone ?? "",
            CarName = r.Car != null ? $"{r.Car.Brand} {r.Car.Model}" : "",
            PlateNumber = r.Car?.PlateNumber ?? "",
            Services = r.ServiceItems
                .Where(si => si.Service != null)
                .Select(si => si.Service.Name)
                .ToList(),
            TotalPrice = r.TotalPrice,
            Status = r.Status.ToString(),
            StatusText = ToStatusText(r.Status),
            CreatedAt = r.CreatedAt
        }).ToList();
    }

    public ServiceRequest? GetById(int id)
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
        var selectedServiceIds = NormalizeServiceIds(vm.SelectedServiceIds);
        EnsureValidServiceSelection(selectedServiceIds);

        using var transaction = _context.Database.BeginTransaction();

        var request = new ServiceRequest
        {
            CarId = vm.SelectedCarId,
            UserId = vm.SelectedUserId,
            Description = vm.Description,
            CreatedAt = DateTime.Now,
            Status = ServiceRequestStatus.New
        };

        _context.ServiceRequests.Add(request);
        _context.SaveChanges();

        var servicePrices = _context.Services
            .Where(s => selectedServiceIds.Contains(s.Id))
            .ToDictionary(s => s.Id, s => s.Price);

        var items = selectedServiceIds.Select(serviceId => new ServiceItem
        {
            ServiceRequestId = request.Id,
            ServiceId = serviceId,
            Price = servicePrices[serviceId]
        }).ToList();

        _context.ServiceItems.AddRange(items);
        request.TotalPrice = items.Sum(i => i.Price);

        _context.SaveChanges();
        transaction.Commit();
    }

    public CreateServiceRequestVM? GetEditVM(int id)
    {
        var request = _context.ServiceRequests
            .Include(r => r.ServiceItems)
            .FirstOrDefault(r => r.Id == id);

        if (request == null)
            return null;

        return new CreateServiceRequestVM
        {
            SelectedCarId = request.CarId,
            SelectedUserId = request.UserId,
            SelectedServiceIds = request.ServiceItems.Select(s => s.ServiceId).ToList(),
            Description = request.Description,
            Cars = _context.Cars.ToList(),
            Services = _context.Services.ToList(),
            Users = _context.Users.ToList()
        };
    }

    public void Update(int id, CreateServiceRequestVM vm)
    {
        var selectedServiceIds = NormalizeServiceIds(vm.SelectedServiceIds);
        EnsureValidServiceSelection(selectedServiceIds);

        using var transaction = _context.Database.BeginTransaction();

        var request = _context.ServiceRequests
            .Include(r => r.ServiceItems)
            .FirstOrDefault(r => r.Id == id);

        if (request == null)
            return;

        request.CarId = vm.SelectedCarId;
        request.UserId = vm.SelectedUserId;
        request.Description = vm.Description;

        _context.ServiceItems.RemoveRange(request.ServiceItems);

        var servicePrices = _context.Services
            .Where(s => selectedServiceIds.Contains(s.Id))
            .ToDictionary(s => s.Id, s => s.Price);

        var items = selectedServiceIds.Select(serviceId => new ServiceItem
        {
            ServiceRequestId = request.Id,
            ServiceId = serviceId,
            Price = servicePrices[serviceId]
        }).ToList();

        _context.ServiceItems.AddRange(items);
        request.TotalPrice = items.Sum(i => i.Price);

        _context.SaveChanges();
        transaction.Commit();
    }

    public void Delete(int id)
    {
        var request = _context.ServiceRequests
            .Include(r => r.ServiceItems)
            .FirstOrDefault(r => r.Id == id);

        if (request == null)
            return;

        _context.ServiceItems.RemoveRange(request.ServiceItems);
        _context.ServiceRequests.Remove(request);
        _context.SaveChanges();
    }

    public void UpdateStatus(int id, ServiceRequestStatus status)
    {
        var request = _context.ServiceRequests.FirstOrDefault(r => r.Id == id);

        if (request == null)
            return;

        request.Status = status;
        _context.SaveChanges();
    }

    private static List<int> NormalizeServiceIds(IEnumerable<int>? serviceIds)
    {
        return serviceIds?
            .Where(id => id > 0)
            .Distinct()
            .ToList()
            ?? new List<int>();
    }

    private void EnsureValidServiceSelection(List<int> selectedServiceIds)
    {
        if (selectedServiceIds.Count == 0)
            throw new ArgumentException("Оберіть хоча б одну послугу");

        var existingIds = _context.Services
            .Where(s => selectedServiceIds.Contains(s.Id))
            .Select(s => s.Id)
            .ToHashSet();

        if (existingIds.Count != selectedServiceIds.Count)
            throw new ArgumentException("Обрано неіснуючу послугу");
    }

    private static string ToStatusText(ServiceRequestStatus status)
    {
        return status switch
        {
            ServiceRequestStatus.New => "Нова",
            ServiceRequestStatus.InProgress => "В роботі",
            ServiceRequestStatus.Done => "Готово",
            _ => status.ToString()
        };
    }
}
