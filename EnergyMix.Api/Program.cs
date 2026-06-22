using EnergyMix.Api.Clients;
using EnergyMix.Api.Exceptions;
using EnergyMix.Api.Interfaces;
using EnergyMix.Api.Services;

namespace EnergyMix.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddHttpClient<ICarbonIntensityClient, CarbonIntensityClient>();
        builder.Services.AddScoped<ICarbonIntensityService, CarbonIntensityService>();
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("http://localhost:4200", "https://energy-mix-frontend-hn7h.onrender.com/")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        
        var app = builder.Build();
        
        app.UseHttpsRedirection();
        
        app.UseCors("AllowFrontend");
        
        app.UseExceptionHandler();

        app.UseAuthorization();
        
        app.MapControllers();
        
        app.MapGet("/", () => "Energy Mix API is running");
        
        app.Run();
    }
}