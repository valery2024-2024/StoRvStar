using Microsoft.AspNetCore.Mvc;
using StoRvStar.Models.Entities;
using StoRvStar.Services.Interfaces;
using StoRvStar.Models.ViewModels;

namespace StoRvStar.Controllers;

public class ServiceRequestController : Controller
{
    private readonly IServiceRequestService _service;

    public ServiceRequestController(IServiceRequestService service)
    {
        _service = service;
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
            Services = _service.GetServices()
        };

        return View(vm);
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