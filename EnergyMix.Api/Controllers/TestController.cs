using EnergyMix.Api.Interfaces;
using EnergyMix.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnergyMix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController(ICarbonIntensityClient client, ICarbonIntensityService carbonIntensityService) : ControllerBase
{
    [HttpGet("/test")]
    public async Task <IActionResult> Get([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var result = await client.GetCarbonIntensityDataAsync(from, to);
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await carbonIntensityService.GetEnergyMixAsync(3);

        return Ok(result);
    }
}