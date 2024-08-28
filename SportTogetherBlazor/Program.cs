using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.ResponseCompression;
using SportTogetherBlazor.Components;
using SportTogetherBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<CircuitHandler, CustomCircuitHandler>();

builder.Services.AddScoped<AuthServices>();
builder.Services.AddScoped<SessionStorageServices>();
builder.Services.AddScoped<AnnonceImageServices>();
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
builder.Services.AddSession(options =>
{
    // Configurez les options de session selon vos besoins
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
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
// Ajout des services requis
builder.Services.AddHttpContextAccessor();
// Ajout de IDistributedCache pour le support des sessions distribuées
builder.Services.AddDistributedMemoryCache(); // Vous pouvez changer cela pour utiliser un autre cache distribué si nécessaire

// Ajout des services Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddBlazorBootstrap();

var app = builder.Build();
app.UseSession();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();


//begin SetString() hack
app.Use(async delegate (HttpContext Context, Func<Task> Next)
{
    //this throwaway session variable will "prime" the SetString() method
    //to allow it to be called after the response has started
    var TempKey = Guid.NewGuid().ToString(); //create a random key
    Context.Session.Set(TempKey, Array.Empty<byte>()); //set the throwaway session variable
    Context.Session.Remove(TempKey); //remove the throwaway session variable
    await Next(); //continue on with the request
});
//end SetString() hack
app.UseRouting();
app.UseAntiforgery();
app.UseResponseCaching();
// Assurez-vous que UseSession est appelé avant MapRazorComponents
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGet("/", context =>
{
    context.Response.Redirect("/SportTogether");
    return Task.CompletedTask;
});

app.Run();