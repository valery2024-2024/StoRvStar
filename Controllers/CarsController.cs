using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using StoRvStar.Data;
using StoRvStar.Models.Entities;

namespace StoRvStar.Controllers;

[Authorize(Roles = "Admin,Manager")]
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
    [ValidateAntiForgeryToken]
    public IActionResult Create(Car car)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Users = _context.Users.ToList();
            return View(car);
        }

        _context.Cars.Add(car);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}
