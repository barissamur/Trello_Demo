using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using Tello_Demo.Web.Handler;
using Tello_Demo.Web.Services;

var builder = WebApplication.CreateBuilder(args);


var webRootPath = builder.Environment.WebRootPath;
var logDirectoryPath = Path.Combine(webRootPath, "log");

if (!Directory.Exists(logDirectoryPath))
    Directory.CreateDirectory(logDirectoryPath);

// Serilog yapýlandýrmasý
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File(Path.Combine(logDirectoryPath, "CardOperationsLog-.log"), rollingInterval: RollingInterval.Day));


// Add services to the container.
builder.Services.AddControllersWithViews();

//jwt options
builder.Services.AddSession();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Index";
    options.Cookie.Name = "Authorization";
});


// Serilog configuration
//builder.Host.UseSerilog((ctx, lc) => lc
//    .WriteTo.Console()
//    .WriteTo.File("logs/log-.log", rollingInterval: RollingInterval.Day));


builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});


var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];

builder.Services.AddHttpClient("APIClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<BearerTokenHandler>();


// Registering CardListService to DI container
builder.Services.AddScoped<CardListService>();
builder.Services.AddScoped<CardService>();
builder.Services.AddSingleton<CardLogService>(new CardLogService(logDirectoryPath));

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<BearerTokenHandler>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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
app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
