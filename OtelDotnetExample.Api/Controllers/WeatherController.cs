using Microsoft.AspNetCore.Mvc;
using OtelDotnetExample.Api.Models;

namespace OtelDotnetExample.Api.Controllers
{
    [Route("v1/weather")]
    public class WeatherController(ILogger<WeatherController> logger) : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetWeather()
        {
            return Ok(new WeatherResponse()
            {
                Temperature = 23,
                Humidity = 80,
            });
        } 
    }
}