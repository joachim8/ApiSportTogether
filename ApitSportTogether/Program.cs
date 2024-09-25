//using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.dbContext;
using ApiSportTogether.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.IO.Compression;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();
// Récupérer la chaîne de connexion
string connectionString = configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API SportTogether", Version = "v1" });
});
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
        _ = builder.AllowAnyOrigin() // URL du client autorisé
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

// Ajout des services pour les contrôleurs
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes =
ResponseCompressionDefaults.MimeTypes.Concat(
    new[] { "image/svg+xml" });
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
});
WebApplication app = builder.Build();
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API SportTogether V1");
    });
}
app.UseHttpsRedirection();
builder.WebHost.UseUrls("http://localhost:5000/");
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
app.UseResponseCompression();
app.MapHub<ChatHubSportTogether>("/chatHubSportTogether");
app.MapHub<PublicationHub>("/publicationHub");
app.MapHub<CommentaireHub>("/commentaireHub");

// Si les images sont dans un dossier spécifique à l'intérieur de wwwroot ou à un autre emplacement :
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

// Activation des endpoints pour les contrôleurs
app.MapControllers();

app.Run();
