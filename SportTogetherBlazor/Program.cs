using SportTogetherBlazor.Components;
using SportTogetherBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<AuthServices>();
builder.Services.AddScoped<LocalStorageServices>();
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5000/ApiSportTogether/")
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddBlazorBootstrap();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapGet("/", context =>
{
    context.Response.Redirect("/SportTogether");
    return Task.CompletedTask;
});
app.Run();
