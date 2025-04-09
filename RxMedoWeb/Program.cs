using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RxMedoWeb.Data;
using RxMedoWeb.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Login";
        options.AccessDeniedPath = "/Admin/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(3);
    });

// Add authorization
builder.Services.AddAuthorization();

// Add DbContext with error handling
try
{
    // Try to connect to MySQL
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString)
        ));
}
catch (Exception ex)
{
    // Log the error but continue application startup
    Console.WriteLine($"Database connection error: {ex.Message}");
    
    // Register a dummy DbContext factory that returns null
    builder.Services.AddScoped<ApplicationDbContext>(provider => null);
}

// Register AuthService
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

// Apply migrations at startup
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (dbContext != null)
        {
            Console.WriteLine("Applying database migrations...");
            dbContext.Database.Migrate();
            Console.WriteLine("Database migrations applied successfully.");
            
            // Create default admin user if not exists
            var authService = scope.ServiceProvider.GetRequiredService<AuthService>();
            await authService.CreateDefaultAdminIfNotExistsAsync();
        }
        else
        {
            Console.WriteLine("Database context is null, skipping migrations.");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
