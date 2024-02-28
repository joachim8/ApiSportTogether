//using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.dbContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();
// Récupérer la chaîne de connexion
string connectionString = configuration.GetConnectionString("DefaultConnection")!;

// Ajouter le contexte de base de données
builder.Services.AddDbContext<SportTogetherContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion("8.0.35"), mysqlOptions => mysqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null)));

builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        _ = builder.WithOrigins("http//:localhost:5000") // URL du client autorisé
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("IterIter49000EliasJoachimAAAAAAAAAAAAAAAAAAAAAAAA")),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.FromMinutes(10)
    };
});

// Ajout des services pour les contrôleurs
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});
WebApplication app = builder.Build();
app.UseHttpsRedirection();
builder.WebHost.UseUrls("http://localhost:5000");

app.UseCors("MyCorsPolicy"); // Utiliser la politique CORS


//Étape 2 : Installation des packages nécessaires

//Ajoutez les packages suivants à votre projet :

//bash

//dotnet add package Microsoft.AspNetCore.Authentication
//dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
//dotnet add package System.IdentityModel.Tokens.Jwt
app.UseForwardedHeaders();
app.UseAuthentication();
app.UseAuthorization();

if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    _ = app.UseHsts();
}
// Configuration des endpoints
app.MapGet("/", () => "Api de Sport Together");

// Activation des endpoints pour les contrôleurs
app.MapControllers();

app.Run();
