using Microsoft.AspNetCore.Mvc;
using StoRvStar.Models.Entities;
using StoRvStar.Services.Interfaces;

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

    // форма створення
    public IActionResult Create()
    {
        return View();
    }

    // збереження
    [HttpPost]
    public IActionResult Create(ServiceRequest request)
    {
        _service.Create(request);
        return RedirectToAction("Index");
    }
}