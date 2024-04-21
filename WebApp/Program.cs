using Microsoft.EntityFrameworkCore;
using AppContext = Infrastructure.Contexts.AppContext;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("WebApp_database")));



var app = builder.Build();
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
