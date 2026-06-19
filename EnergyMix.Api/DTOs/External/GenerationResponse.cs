using System.Text.Json.Serialization;

namespace EnergyMix.Api.DTOs.External;

public class GenerationResponse
{
    [JsonPropertyName("data")]
    public List<GenerationInterval> Data { get; set; } = new();
}