using EnergyMix.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnergyMix.Api.Controllers;

[ApiController]
public class TestController(ICarbonIntensityClient client) : ControllerBase
{
    [HttpGet("/test")]
    public async Task <IActionResult> Get([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var result = await client.GetCarbonIntensityDataAsync(from, to);
        return Ok(result);
    }
}