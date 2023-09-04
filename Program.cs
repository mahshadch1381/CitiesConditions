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
using StackExchange.Redis;
using DBFIRST_Cities3.Services;
using System.Diagnostics;
using DBFIRST_Cities3.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("AuthenConnection");
    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,

        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WorldContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MainConnection"));
});

builder.Services.AddTransient<CityPopulationService>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IWeatherService, WeatherService>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IInformationService, RegionService>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<ICityPopulationService, CityPopulationService>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IHumidityService, HumidityService>();

builder.Services.AddTransient<HumidityService>();
builder.Services.AddTransient<RegionService>();
builder.Services.AddTransient<WeatherService>();

var redisServerPath = "RedLocation";
if (File.Exists(redisServerPath))
{
    var processInfo = new ProcessStartInfo
    {
        FileName = redisServerPath,
        UseShellExecute = false,
        RedirectStandardOutput = true,
        CreateNoWindow = true
    };

    var process = new Process { StartInfo = processInfo };
    process.Start();
    using (StreamReader reader = process.StandardOutput)
    {
        string output = reader.ReadToEnd();
        Console.WriteLine(output);
    }
}
else
{
    Console.WriteLine("Redis server executable not found.");
}
var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
//var redisConnectionString = "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));
builder.Services.AddSingleton<IRedisService, RedisService>();
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

