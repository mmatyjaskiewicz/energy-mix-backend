namespace EnergyMix.Api.DTOs.Responses;

public class EnergyMixResponse
{
    public DateOnly Date { get; set; }
    public decimal CleanEnergyPercentage { get; set; }
    public List<EnergySourceResponse> Sources { get; set; } = new();
}