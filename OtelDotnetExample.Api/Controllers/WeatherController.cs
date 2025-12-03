using Microsoft.AspNetCore.Mvc;

namespace OtelDotnetExample.Api.Controllers
{
    [Route("v1/weather")]
    public class WeatherController(ILogger<WeatherController> logger) : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetWeather()
        {
            return Ok("Weather data: 23 degrees");
        } 
    }
}