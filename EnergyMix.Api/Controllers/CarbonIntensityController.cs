using EnergyMix.Api.DTOs.Requests;
using EnergyMix.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnergyMix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarbonIntensityController(ICarbonIntensityService carbonIntensityService) : ControllerBase
{
    [HttpGet("energy-mix")]
    public async Task<IActionResult> GetEnergyMix()
    {
        var result = await carbonIntensityService.GetEnergyMixAsync(3);

        return Ok(result);
    }

    [HttpGet("charging-window")]
    public async Task<IActionResult> GetChargingWindow([FromBody] GetChargingWindowRequest request)
    {
        var result = await carbonIntensityService.GetOptimalChargingWindowAsync(request.Hours);
        return Ok(result);
    }
}