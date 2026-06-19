using System.Text.Json.Serialization;

namespace EnergyMix.Api.DTOs.External;

public class GenerationInterval
{
    [JsonPropertyName("from")]
    public DateTime From { get; set; }
    
    [JsonPropertyName("to")]
    public DateTime To { get; set; }
    
    [JsonPropertyName("generationmix")]
    public List<GenerationMix> GenerationMix { get; set; } = new();
}