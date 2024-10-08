using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.ResponseCompression;
using SportTogetherBlazor.Components;
using SportTogetherBlazor.Services;

var builder = WebApplication.CreateBuilder(args);



// Enregistrer LocalStorageServices
builder.Services.AddScoped<LocalStorageServices>();
builder.Services.AddScoped<TaskAmiServices>();
builder.Services.AddScoped<TaskNotificationServices>();

// Enregistrer HttpClient et IHttpClientFactory avec la base URL
builder.Services.AddHttpClient("ApiSportTogetherClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000/ApiSportTogether/");
});

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiSportTogetherClient"));

builder.Services.Configure<CookiePolicyOptions>(options =>
{

    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});


builder.Services.AddResponseCompression(opts =>
{
    opts.EnableForHttps = true;
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
    {
        "application/octet-stream"
    });
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options =>
       {
           options.ExpireTimeSpan = TimeSpan.FromDays(7);
           options.SlidingExpiration = true;
           options.Cookie.HttpOnly = true;
           options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
       });
// Configuration pour augmenter la taille maximale des fichiers
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10485760; // 10 MB
});

// Ajout de IDistributedCache pour le support des sessions distribuées
builder.Services.AddDistributedMemoryCache(); // Vous pouvez changer cela pour utiliser un autre cache distribué si nécessaire

// Ajout des services Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBlazorBootstrap();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

//end SetString() hack
app.UseRouting();
app.UseAntiforgery();
app.UseResponseCaching();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGet("/", context =>
{
    context.Response.Redirect("/SportTogether");
    return Task.CompletedTask;
});

app.Run();