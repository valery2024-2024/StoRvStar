using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoRvStar.Models.Entities;
using StoRvStar.Services.Interfaces;
using StoRvStar.Models.ViewModels;
using StoRvStar.Data;

namespace StoRvStar.Controllers;

public class ServiceRequestController : Controller
{
    private readonly IServiceRequestService _service;
    private readonly AppDbContext _context;

    public ServiceRequestController(IServiceRequestService service, AppDbContext context)
    {
        _service = service;
        _context = context;
    }

    // список заявок
    public IActionResult Index()
    {
        var requests = _service.GetAll();
        return View(requests);
    }

    // GET - форма створення
    public IActionResult Create()
    {
        var vm = new CreateServiceRequestVM
        {
            Cars = _service.GetCars(),
            Services = _service.GetServices(),
            Users = _context.Users.ToList()
        };

        return View(vm);
    }

    // зміна статусу
    public IActionResult UpdateStatus(int id, string status)
    {
        _service.UpdateStatus(id, status);
        return RedirectToAction("Index");
    }

    // POST - створення
    [HttpPost]
    public IActionResult Create(CreateServiceRequestVM vm)
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

        return RedirectToAction("Index");
    }

    // EDIT - GET
    public IActionResult Edit(int id)
    {
        var request = _context.ServiceRequests
            .Include(r => r.ServiceItems)
            .FirstOrDefault(r => r.Id == id);

        if (request == null) return NotFound();

        var vm = new CreateServiceRequestVM
        {
            SelectedCarId = request.CarId,
            SelectedUserId = request.UserId,
            SelectedServiceIds = request.ServiceItems.Select(s => s.ServiceId).ToList(),

            Cars = _context.Cars.ToList(),
            Services = _context.Services.ToList(),
            Users = _context.Users.ToList()
        };

        ViewBag.Id = id;

        return View(vm);
    }

    // EDIT - POST
    [HttpPost]
    public IActionResult Edit(int id, CreateServiceRequestVM vm)
    {
        var request = _context.ServiceRequests
            .Include(r => r.ServiceItems)
            .FirstOrDefault(r => r.Id == id);

        if (request == null) return NotFound();

        request.CarId = vm.SelectedCarId;
        request.UserId = vm.SelectedUserId;
        request.Description = vm.Description;

        // видаляємо старі послуги
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

        return RedirectToAction("Index");
    }

    // DELETE
    public IActionResult Delete(int id)
    {
        var request = _context.ServiceRequests
            .Include(r => r.ServiceItems)
            .FirstOrDefault(r => r.Id == id);

        if (request == null) return NotFound();

        _context.ServiceItems.RemoveRange(request.ServiceItems);
        _context.ServiceRequests.Remove(request);

        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}