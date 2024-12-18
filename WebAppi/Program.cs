using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Ajouter la configuration pour utiliser les contrôleurs
builder.Services.AddControllers();

// Configuration du DbContext avec la chaîne de connexion définie dans appsettings.json
builder.Services.AddDbContext<BankingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuration pour Swagger (documentation de l'API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transactions API", Version = "v1" });

    // Ajoutez cette ligne pour permettre l'utilisation de corps dans les requêtes
    c.DescribeAllParametersInCamelCase();
    c.OperationFilter<SwaggerBodyFixOperationFilter>();
});

var app = builder.Build();

// Configuration de Swagger pour générer et servir la documentation de l'API
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



// Middleware pour gérer les requêtes HTTP
app.UseRouting();

// Autoriser les contrôleurs
app.UseAuthorization();

// Mappez les contrôleurs
app.MapControllers();

// Démarrez l'application
app.Run();