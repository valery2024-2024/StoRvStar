using StoRvStar.Data;
using Microsoft.EntityFrameworkCore;
using StoRvStar.Services;
using StoRvStar.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using StoRvStar.Models.Identity;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=storvstar.db"));

// Identity seed options
builder.Services.Configure<IdentitySeedOptions>(
    builder.Configuration.GetSection(IdentitySeedOptions.SectionName));

// Identity (ОСНОВА АВТОРИЗАЦІЇ)
builder.Services
    .AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;

        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Куди перекидає якщо не авторизований
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Мої сервіси
builder.Services.AddScoped<IServiceRequestService, ServiceRequestService>();

var app = builder.Build();

// СТВОРЕННЯ РОЛЕЙ І DEV-АДМІНА
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    await IdentitySeeder.SeedAsync(services);
}

// Configure pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
