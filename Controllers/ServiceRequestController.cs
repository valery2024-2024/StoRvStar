using Microsoft.AspNetCore.Mvc;
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

    public IActionResult UpdateStatus(int id, string status)
    {
        _service.UpdateStatus(id, status);
        return RedirectToAction("Index");
    }

    // POST - форма збереження
    [HttpPost]
    public IActionResult Create(CreateServiceRequestVM vm)
    {
        // створюємо заявку
        var request = new ServiceRequest
        {
            CarId = vm.CarId,
            UserId = vm.UserId,
            CreatedAt = DateTime.Now,
            Status = "New"
        };

        // створюємо заявку
        _service.Create(request, vm.SelectedServices);


        return RedirectToAction("Index");
    }
    
}