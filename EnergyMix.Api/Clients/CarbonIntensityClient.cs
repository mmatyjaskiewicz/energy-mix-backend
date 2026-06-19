using System.Text.Json;
using EnergyMix.Api.DTOs.External;
using EnergyMix.Api.Interfaces;

namespace EnergyMix.Api.Clients;

public class CarbonIntensityClient(HttpClient httpClient) : ICarbonIntensityClient
{
    public async Task<GenerationResponse> GetCarbonIntensityDataAsync(DateTime from, DateTime to)
    {
        var fromString = from.ToString("yyyy-MM-ddTHH:mmZ");
        var toString = to.ToString("yyyy-MM-ddTHH:mmZ");
        
        var url = $"https://api.carbonintensity.org.uk/generation/{fromString}/{toString}";
        
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<GenerationResponse>(json, new JsonSerializerOptions { WriteIndented = true });
        
        return result ?? new GenerationResponse();
    }
}