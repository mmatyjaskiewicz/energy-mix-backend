using EnergyMix.Api.DTOs.External;
using EnergyMix.Api.DTOs.Responses;
using EnergyMix.Api.DTOs.Results;
using EnergyMix.Api.Interfaces;

namespace EnergyMix.Api.Services;

public class CarbonIntensityService(ICarbonIntensityClient carbonIntensityClient) : ICarbonIntensityService
{
    public async Task<List<EnergyMixResponse>> GetEnergyMixAsync(int days)
    {
        var dailyData = await GetGenerationDataAsync(days);

        var result = dailyData
            .Select(CalculateEnergyMix)
            .ToList();

        return result;
    }
    
    private EnergyMixResponse CalculateEnergyMix(DailyGenerationData day)
    {
        var mixes = day.Intervals.SelectMany(interval => interval.GenerationMix);
        
        var groupedByFuel = mixes.GroupBy(x => x.Fuel);
        
        var averages = groupedByFuel
            .Select(group => new EnergySourceResponse
            {
                Fuel = group.Key,
                AveragePercentage = group.Average(x => x.Perc)
            })
            .ToList();

        var cleanEnergyPercentage = averages
            .Where(x =>
                x.Fuel == "biomass" ||
                x.Fuel == "nuclear" ||
                x.Fuel == "hydro" ||
                x.Fuel == "solar" ||
                x.Fuel == "wind")
            .Sum(x => x.AveragePercentage);

        return new EnergyMixResponse
        {
            Date = DateOnly.FromDateTime(day.Intervals.First().From),
            Sources = averages,
            CleanEnergyPercentage = cleanEnergyPercentage
        };
    }
    
    private async Task<List<DailyGenerationData>> GetGenerationDataAsync(int days)
    {
        var from = DateTime.UtcNow;
        var to = from.AddDays(days);
        
        var rawData = await carbonIntensityClient.GetCarbonIntensityDataAsync(from, to);
        
        return SplitByDay(rawData);
    }
    
    private List<DailyGenerationData> SplitByDay(GenerationResponse dataToSplit)
    {
        var groupedByDay = dataToSplit.Intervals.GroupBy(x => x.From.Date);
        
        var dailyResponses = groupedByDay
            .Select(day => new DailyGenerationData
            {
                Intervals = day.ToList()
            })
            .ToList();
        
        return dailyResponses;
    }
}