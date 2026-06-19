using EnergyMix.Api.DTOs.External;

namespace EnergyMix.Api.Interfaces;

public interface ICarbonIntensityClient
{
    public Task<GenerationResponse> GetCarbonIntensityDataAsync(DateTime from, DateTime to);
}