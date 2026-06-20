using EnergyMix.Api.DTOs.External;
using EnergyMix.Api.DTOs.Responses;
using EnergyMix.Api.DTOs.Results;
using EnergyMix.Api.Interfaces;

namespace EnergyMix.Api.Services;

public class CarbonIntensityService(ICarbonIntensityClient carbonIntensityClient) : ICarbonIntensityService
{
    // 2nd Endpoint
    public async Task<ChargingWindowResponse> GetOptimalChargingWindowAsync(int hours)
    {
        if (hours < 1 || hours > 6)
        {
            throw new ArgumentException("Hours must be between 1 and 6.");
        }
        
        var dailyData = await GetGenerationDataAsync(2);
        
        var intervals = dailyData
            .SelectMany(day => day.Intervals)
            .OrderBy(x => x.From)
            .ToList();
        
        var intervalPercentages = intervals
            .Select(x => new
            {
                Interval = x,
                CleanEnergyPercentage = CalculateCleanEnergyPercentage(x)
            })
            .ToList();
        
        var windowSize = hours * 2;
        
        decimal bestAverage = 0;
        int bestStartIndex = 0;

        for (var i = 0; i <= intervalPercentages.Count - windowSize; i++)
        {
            var currentAverage = intervalPercentages
                .Skip(i)
                .Take(windowSize)
                .Average(x => x.CleanEnergyPercentage);

            if (currentAverage > bestAverage)
            {
                bestAverage = currentAverage;
                bestStartIndex = i;
            }
        }
        
        var startInterval = intervals[bestStartIndex];
        var endInterval = intervals[bestStartIndex + windowSize - 1];

        return new ChargingWindowResponse
        {
            Start = startInterval.From,
            End = endInterval.To,
            AverageCleanEnergyPercentage = Math.Round(bestAverage, 2)
        };
    }
    
    
    private decimal CalculateCleanEnergyPercentage(GenerationInterval interval)
    {
        var cleanEnergySources = new List<string> { "biomass", "nuclear", "hydro", "solar", "wind" };
        
        var cleanEnergyPercentage = interval.GenerationMix
            .Where(x => cleanEnergySources.Contains(x.Fuel))
            .Sum(x => x.Perc);
        
        return Math.Round(cleanEnergyPercentage, 2);
    }
    
    
    // 1st Endpoint
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
                AveragePercentage = Math.Round(group.Average(x => x.Perc),2)
            })
            .ToList();

        var cleanEnergyPercentage = averages
            .Where(x => x.Fuel == "biomass" || x.Fuel == "nuclear" || x.Fuel == "hydro" || x.Fuel == "solar" || x.Fuel == "wind")
            .Sum(x => x.AveragePercentage);

        return new EnergyMixResponse
        {
            Date = DateOnly.FromDateTime(day.Intervals.First().From),
            Sources = averages,
            CleanEnergyPercentage = Math.Round(cleanEnergyPercentage, 2)
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