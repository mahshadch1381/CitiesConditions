using DBFIRST_Cities3.Models;
using First_Project.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using DBFIRST_Cities3.relation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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

builder.Services.AddLogging();

app.Use(async (context, next) =>
{
    var currentDate = DateTime.Now.ToString("yyyy-MM-dd");
    var logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
    var logFilePath = Path.Combine(logDirectory, $"{currentDate}.log");

    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    var originalBodyStream = context.Response.Body;

    Directory.CreateDirectory(logDirectory);

    using var responseBody = new MemoryStream();
    context.Response.Body = responseBody;

    await next.Invoke();

    string logLevel;
    if (context.Response.StatusCode >= 500)
    {
        logLevel = "Error";
    }
    else if (context.Response.StatusCode >= 400)
    {
        logLevel = "Warning";
    }
    else
    {
        logLevel = "Information";
    }

    var requestMessage = $"Request: {context.Request.Method} {context.Request.Path}";
    logger.LogInformation($"{logLevel}: {requestMessage}");

    responseBody.Seek(0, SeekOrigin.Begin);

    await responseBody.CopyToAsync(originalBodyStream);
    context.Response.Body = originalBodyStream;

    var statusCode = context.Response.StatusCode;
    var reasonPhrase = Status.GetReasonPhrase(statusCode);
    var responseMessage = $"Response: {statusCode} {reasonPhrase}";
    logger.LogInformation($"{logLevel}: {responseMessage}");

    using (var writer = new StreamWriter(logFilePath, append: true))
    {
        writer.Write($"{logLevel}: {requestMessage} , ");
        writer.WriteLine("request time : "+ DateTime.Now);
        writer.WriteLine($"{logLevel}: {responseMessage}");
        writer.WriteLine();
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();