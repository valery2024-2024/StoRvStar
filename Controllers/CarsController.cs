using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using StoRvStar.Data;
using StoRvStar.Models.Entities;

namespace StoRvStar.Controllers;

public class CarsController : Controller
{
    private readonly AppDbContext _context;

    public CarsController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var cars = _context.Cars
            .Include(c => c.User)
            .ToList();

        return View(cars);
    }

    public IActionResult Create()
    {
        ViewBag.Users = _context.Users.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult Create(Car car)
    {
        _context.Cars.Add(car);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}