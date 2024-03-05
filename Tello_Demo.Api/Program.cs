using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tello_Demo.Application.Interfaces;
using Tello_Demo.Application.Mapping.CardListMapping;
using Tello_Demo.Application.Mapping.CardMapping;
using Tello_Demo.Application.Services;
using Tello_Demo.Infrastructure.Context;
using Tello_Demo.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContext") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// iç içe döngü olarak getirme sorunu için
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});


// injection
builder.Services.AddScoped<ICardListService, CardListService>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped(typeof(IRepo<>), typeof(EFRepository<>));

//AUTOMAPPER
var config = new MapperConfiguration(conf =>
{
    conf.AddProfile<CardListMappingProfile>();
    conf.AddProfile<CardMappingProfile>();
});
builder.Services.AddScoped(s => config.CreateMapper());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
