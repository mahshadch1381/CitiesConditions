using DBFIRST_Cities3.Models;
using First_Project.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WorldContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Data Source=ACADEMY11\\SQLEXPRESS;Initial Catalog=world;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));
});

builder.Services.AddHttpClient();
builder.Services.AddScoped<IWeatherService, WeatherService>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IInformationService, RegionService>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<ICityPopulationService, CityPopulationService>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IHumidityService, HumidityService>();
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
