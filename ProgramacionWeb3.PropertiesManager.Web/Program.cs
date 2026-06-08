using ProgramacionWeb3.PropertiesManager.Core;
using ProgramacionWeb3.PropertiesManager.Infrastructure;
using ProgramacionWeb3.PropertiesManager.Web;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddInfrastructure(connectionString);
builder.Services.AddCore();
builder.Services.AddWeb();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "expensesIndex",
    pattern: "Expenses/Index/{buildingId:int}",
    defaults: new { controller = "Expenses", action = "Index" });

app.MapControllerRoute(
    name: "expensesCreate",
    pattern: "Expenses/Create/{buildingId:int}",
    defaults: new { controller = "Expenses", action = "Create" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
