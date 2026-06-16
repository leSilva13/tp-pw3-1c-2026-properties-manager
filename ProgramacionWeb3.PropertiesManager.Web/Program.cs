using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProgramacionWeb3.PropertiesManager.Core;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces;
using ProgramacionWeb3.PropertiesManager.Infrastructure;
using ProgramacionWeb3.PropertiesManager.Web;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUnitOfWork>(provider =>
    provider.GetRequiredService<AppDbContext>());

string filesBasePath = builder.Environment.ContentRootPath;

builder.Services.AddInfrastructure(connectionString, filesBasePath);
builder.Services.AddCore();
builder.Services.AddWeb();

builder.Services.AddControllersWithViews();
// Cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login";
        options.Cookie.HttpOnly = true;
        options.Cookie.Name = "PropertiesManager.Auth";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Shows the raw code crash when you are testing locally
    app.UseDeveloperExceptionPage();
}
else
{
    // Catches the error and silently redirects the user to the Error page
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
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

app.MapControllerRoute(
    name: "notificationsIndex",
    pattern: "Notifications/Index/{buildingId:int?}",
    defaults: new { controller = "Notifications", action = "Index" });

app.MapControllerRoute(
    name: "notificationsCreate",
    pattern: "Notifications/Create/{buildingId:int?}",
    defaults: new { controller = "Notifications", action = "Create" });


app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
