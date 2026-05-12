using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoRvStar.Services.Interfaces;
using StoRvStar.Models.ViewModels;

namespace StoRvStar.Controllers;

[Authorize(Roles = "Admin,Manager")]
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
        var requests = _service.GetAllForIndex();
        return View(requests);
    }

    // DETAILS
    public IActionResult Details(int id)
    {
        var request = _service.GetById(id);

        if (request == null)
            return NotFound();

        return View(request);
    }

    // GET - форма створення
    public IActionResult Create()
    {
        var vm = new CreateServiceRequestVM
        {
            Cars = _service.GetCars(),
            Services = _service.GetServices(),
            Users = _service.GetUsers()
        };

        return View(vm);
    }

    // POST - створення
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateServiceRequestVM vm)
    {
        _service.Create(vm);
        return RedirectToAction("Index");
    }

    // EDIT - GET
    public IActionResult Edit(int id)
    {
        var vm = _service.GetEditVM(id);

        if (vm == null)
            return NotFound();

        ViewBag.Id = id;

        return View(vm);
    }

    // EDIT - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, CreateServiceRequestVM vm)
    {
        _service.Update(id, vm);
        return RedirectToAction("Index");
    }

    // DELETE
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        _service.Delete(id);
        return RedirectToAction("Index");
    }

    // зміна статусу
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateStatus(int id, string status)
    {
        var allowedStatuses = new[] { "InProgress", "Done" };

        if (!allowedStatuses.Contains(status))
        {
            return BadRequest();
        }

        _service.UpdateStatus(id, status);
        return RedirectToAction("Index");
    }
}
