using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoRvStar.Models.Enums;
using StoRvStar.Services.Interfaces;
using StoRvStar.Models.ViewModels;

namespace StoRvStar.Controllers;

[Authorize(Roles = "Admin,Manager")]
public class ServiceRequestController : Controller
{
    private static readonly HashSet<ServiceRequestStatus> AllowedStatuses = new()
    {
        ServiceRequestStatus.InProgress,
        ServiceRequestStatus.Done
    };

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
        if (!ModelState.IsValid)
        {
            PopulateLookups(vm);
            return View(vm);
        }

        try
        {
            _service.Create(vm);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            PopulateLookups(vm);
            return View(vm);
        }

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
        if (!ModelState.IsValid)
        {
            PopulateLookups(vm);
            ViewBag.Id = id;
            return View(vm);
        }

        try
        {
            _service.Update(id, vm);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            PopulateLookups(vm);
            ViewBag.Id = id;
            return View(vm);
        }

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
        if (!Enum.TryParse<ServiceRequestStatus>(status, true, out var parsedStatus) ||
            !AllowedStatuses.Contains(parsedStatus))
            return BadRequest("Некоректний статус");

        _service.UpdateStatus(id, parsedStatus);
        return RedirectToAction("Index");
    }

    private void PopulateLookups(CreateServiceRequestVM vm)
    {
        vm.Cars = _service.GetCars();
        vm.Services = _service.GetServices();
        vm.Users = _service.GetUsers();
    }
}
