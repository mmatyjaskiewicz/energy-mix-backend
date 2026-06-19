using System.Text.Json.Serialization;

namespace EnergyMix.Api.DTOs.External;

public class GenerationMix
{
    [JsonPropertyName("fuel")]
    public string Fuel { get; set; } = string.Empty;
    
    [JsonPropertyName("perc")]
    public decimal Perc { get; set; }
}