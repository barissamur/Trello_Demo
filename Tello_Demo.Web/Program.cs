using Serilog;
using Tello_Demo.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Serilog configuration
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("logs/log-.log", rollingInterval: RollingInterval.Day));

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Yapýlandýrmadan BaseUrl deðerini okuyun
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"]; // Bu satýrý düzelttim

// HttpClient servisini kaydedin ve BaseAddress olarak ApiSettings:BaseUrl kullanýn
builder.Services.AddHttpClient("APIClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// Registering CardListService to DI container
builder.Services.AddScoped<CardListService>();
builder.Services.AddScoped<CardService>();
builder.Services.AddScoped<TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
