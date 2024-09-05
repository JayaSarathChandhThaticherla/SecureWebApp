using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Models;
using MyApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure the DbContext to use an in-memory database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("MyAppDb"));

// Configure Identity with custom ApplicationUser and role management
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Register repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBooksRepository, BookRepository>();

// Configure cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Path to the login page
    options.AccessDeniedPath = "/Account/AccessDenied"; // Path to access denied page
    options.LogoutPath = "/Account/Logout"; // Path to logout page
});

var app = builder.Build();

// Seed data (roles and admin user)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userRepository = services.GetRequiredService<IUserRepository>();
        await userRepository.EnsureRolesCreatedAsync();
        await SeedData.SeedRolesAndAdmin(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => {
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Login}/{id?}");
});


app.Run();
