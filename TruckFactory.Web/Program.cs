global using Microsoft.EntityFrameworkCore;
using TruckFactory.Web.Domain.Repositories;
using TruckFactory.Web.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<FactoryContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TruckApp"));
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

