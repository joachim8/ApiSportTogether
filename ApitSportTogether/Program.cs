//using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.dbContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();
// R�cup�rer la cha�ne de connexion
string connectionString = configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API SportTogether", Version = "v1" });
});
// Ajouter le contexte de base de donn�es
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
        _ = builder.AllowAnyOrigin() // URL du client autoris�
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SportTogetherJoachimAAAAAAAAAAAAAAAAAAAAAAAA")),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.FromMinutes(60)
    };
});

// Ajout des services pour les contr�leurs
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});
// Configuration pour augmenter la taille maximale des fichiers
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10485760; // 10 MB
    options.BufferBodyLengthLimit = 10485760; // 10 MB buffer limit
});
WebApplication app = builder.Build();
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API ITER ITER V1");
    });
}
app.UseHttpsRedirection();
builder.WebHost.UseUrls("http://localhost:5000/");
app.UseCors("MyCorsPolicy"); // Utiliser la politique CORS


//�tape 2 : Installation des packages n�cessaires

//Ajoutez les packages suivants � votre projet :

//bash

//dotnet add package Microsoft.AspNetCore.Authentication
//dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
//dotnet add package System.IdentityModel.Tokens.Jwt
app.UseForwardedHeaders();
app.UseAuthentication();
app.UseAuthorization();


// Si les images sont dans un dossier sp�cifique � l'int�rieur de wwwroot ou � un autre emplacement :
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(app.Environment.ContentRootPath, "Images")),
    RequestPath = "/Images"
});
if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    _ = app.UseHsts();
}
// Configuration des endpoints
app.MapGet("/", () => "Api de Sport.");

// Activation des endpoints pour les contr�leurs
app.MapControllers();

app.Run();
