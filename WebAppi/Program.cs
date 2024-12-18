using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Ajouter la configuration pour utiliser les contr�leurs
builder.Services.AddControllers();

// Configuration du DbContext avec la cha�ne de connexion d�finie dans appsettings.json
builder.Services.AddDbContext<BankingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuration pour Swagger (documentation de l'API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transactions API", Version = "v1" });

    // Ajoutez cette ligne pour permettre l'utilisation de corps dans les requ�tes
    c.DescribeAllParametersInCamelCase();
    c.OperationFilter<SwaggerBodyFixOperationFilter>();
});

var app = builder.Build();

// Configuration de Swagger pour g�n�rer et servir la documentation de l'API
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



// Middleware pour g�rer les requ�tes HTTP
app.UseRouting();

// Autoriser les contr�leurs
app.UseAuthorization();

// Mappez les contr�leurs
app.MapControllers();

// D�marrez l'application
app.Run();