using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Smo.Wmi;
using TEST_PFE.Models;  // Assurez-vous que le namespace de vos modèles est correctement référencé
using TEST_PFE.Ser;    // Assurez-vous d'ajouter ce namespace pour CrmService

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logging => logging.AddConsole());

// Configure le DbContext avec SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure CrmService avec l'injection de la configuration
builder.Services.AddSingleton<CrmService>();

// Autres services nécessaires
builder.Services.AddControllersWithViews();

// Configuration de l'accès à la base de données et aux services nécessaires
builder.Services.AddSingleton<IConfiguration>(builder.Configuration); // Si tu n'as pas déjà injecté IConfiguration

var app = builder.Build();

// Configure le pipeline de requêtes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Mappe les routes des contrôleurs MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
