using EnergyMix.Api.Interfaces;
using EnergyMix.Api.Services;

namespace EnergyMix.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
        builder.Services.AddHttpClient<ICarbonIntensityClient, CarbonIntensityClient>();
        
        var app = builder.Build();
        
        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapControllers();
        
        app.Run();
    }
}