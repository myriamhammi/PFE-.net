using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Smo.Wmi;
using System.ComponentModel;
using TEST_PFE.Models;  
using TEST_PFE.Ser;    


System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


var builder = WebApplication.CreateBuilder(args);




builder.Services.AddLogging(logging => logging.AddConsole());

// Configure le DbContext avec SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Lire la chaîne de connexion depuis appsettings.json
var connectionString = builder.Configuration.GetConnectionString("SQL_Connection");




// Configure CrmService avec l'injection de la configuration
builder.Services.AddSingleton<CrmService>();

// Autres services nécessaires
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IChargementService, ChargementService>();
builder.Services.AddScoped<IDataTransformationService, DataTransformationService>();
builder.Services.AddScoped<DynamicsService>();
builder.Services.AddScoped<DataSyncService>();
builder.Services.AddScoped <OAuthHelper>();

builder.Services.AddHttpClient();


// Configuration de l'accès à la base de données et aux services nécessaires
builder.Services.AddSingleton<IConfiguration>(builder.Configuration); // Si tu n'as pas déjà injecté IConfiguration


// Forcer la redirection HTTP vers HTTPS
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 7190;  // Port HTTPS que vous utilisez
});


builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Toujours sécuriser les cookies
    options.Cookie.SameSite = SameSiteMode.Strict;  // Prévenir l'envoi de cookies dans les requêtes inter-domaines
});


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
