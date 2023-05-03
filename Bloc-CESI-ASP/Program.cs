using Bloc_CESI_ASP.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("MySql");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 31)))) ;

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.Use(async (context, next) =>
{
    if (!context.Request.Cookies.ContainsKey("userToken") && context.Request.Path.StartsWithSegments("/admin") )
    {
        context.Response.Redirect("/login");
    }
    await next();
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "admin_employees",
    pattern: "admin/employees/{action=Index}/{id?}",
    defaults: new { controller = "Employees"});

app.MapControllerRoute(
    name: "admin_sites",
    pattern: "admin/sites/{action=Index}/{id?}",
    defaults: new { controller = "Sites"});

app.MapControllerRoute(
    name: "admin_services",
    pattern: "admin/services/{action=Index}/{id?}",
    defaults: new { controller = "Services"});

app.MapControllerRoute(
    name: "login",
    pattern: "login",
    defaults: new { controller = "Users", action = "Login" });

app.MapControllerRoute(
    name: "register",
    pattern: "register",
    defaults: new { controller = "Users", action = "Register" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();