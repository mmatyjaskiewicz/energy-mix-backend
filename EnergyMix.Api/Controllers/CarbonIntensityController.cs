using EnergyMix.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnergyMix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarbonIntensityController(ICarbonIntensityService carbonIntensityService) : ControllerBase
{
    [HttpGet("energy-mix")]
    public async Task<IActionResult> Get()
    {
        var result = await carbonIntensityService.GetEnergyMixAsync(3);

        return Ok(result);
    }
}