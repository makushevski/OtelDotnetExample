using Microsoft.AspNetCore.Mvc;
using OtelDotnetExample.Api.DataLayer;
using OtelDotnetExample.Api.Models;

namespace OtelDotnetExample.Api.Controllers
{
    [Route("v1/weather")]
    public class WeatherController(PgContext dbContext, ILogger<WeatherController> logger) : ControllerBase
    {
        public PgContext DbContext { get; } = dbContext;
        private readonly ILogger<WeatherController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetWeather([FromQuery] bool throwException = false)
        {
            if (throwException)
            {
                return BadRequest("Error getting weather data");
            }
            
            var first = DbContext.Weather.First();
            return Ok(new WeatherResponse()
            {
                Temperature = first.Temperature,
                Humidity = first.Humidity,
            });
        }
    }
}