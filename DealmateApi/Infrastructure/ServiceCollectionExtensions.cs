using DealmateApi.Domain.PredicateBuilders;
using DealmateApi.Infrastructure.DB;
using DealmateApi.Infrastructure.Interfaces;
using DealmateApi.Infrastructure.Repositories;
using DealmateApi.Service.Common;
using DealmateApi.Service.Enforcer;
using DealmateApi.Service.ExcelProcess;
using DealmateApi.Service.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace DealmateApi.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection InfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddHttpContextAccessor();
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
            };
        });
        services.AddSwaggerGen(swagger =>
        {
            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header
            });
            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id= "Bearer"
                        }
                    },Array.Empty<string>()
                }
            });
        });
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IDealerRepository, DealerRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
        services.AddScoped<IEnforcer, EnforcerService>();
        services.AddScoped<IExcelService, ExcelService>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<VehicleFilterPredicateBuilder>();
        services.AddControllers(options =>
        {
            options.Filters.Add<HttpResponseExceptionFilter>(); // Add the custom exception filter
        });
        return services;
    }
}
