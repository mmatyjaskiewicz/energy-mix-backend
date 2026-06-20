namespace EnergyMix.Api.DTOs.Responses;

public class ChargingWindowResponse
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public decimal AverageCleanEnergyPercentage { get; set; }
}