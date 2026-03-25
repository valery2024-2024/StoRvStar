using Microsoft.EntityFrameworkCore;
using StoRvStar.Models.Entities;

namespace StoRvStar.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<ServiceRequest> ServiceRequests { get; set; }
    public DbSet<ServiceItem> ServiceItems { get; set; }
    public DbSet<Review> Reviews { get; set; }
}