using DealmateApi.Infrastructure;
using DealmateApi.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Determine the environment
var environment = builder.Environment.EnvironmentName;
// Load the base configuration from appsettings.json
var configuration = builder.Configuration;

var envConfigSection = configuration.GetSection($"Environments:{environment}");
builder.Configuration.AddConfiguration(envConfigSection);
// Add services to the container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddAzureWebAppDiagnostics();

builder.Services.InfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowSpecificOrigins");
//app.UseMultiTenant();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Handle fallback for SPA routes
app.MapFallbackToFile("index.html");

app.Run();
