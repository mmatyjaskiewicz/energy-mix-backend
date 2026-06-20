using EnergyMix.Api.DTOs.Responses;


namespace EnergyMix.Api.Interfaces;

public interface ICarbonIntensityService
{
    public Task<List<EnergyMixResponse>> GetEnergyMixAsync(int days);
    public Task<ChargingWindowResponse> GetOptimalChargingWindowAsync(int hours);
}