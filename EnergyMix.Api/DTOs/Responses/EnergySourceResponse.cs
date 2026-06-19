namespace EnergyMix.Api.DTOs.Responses;

public class EnergySourceResponse
{
    public string Fuel { get; set; } = string.Empty;
    public decimal AveragePercentage { get; set; }
}