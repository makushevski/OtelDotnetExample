using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OtelDotnetExample.Api.Client;

namespace OtelDotnetExample.Gateway.Controllers
{
    [Route("v1/weather")]
    public class WeatherController(IOtelDotnetExampleApiClient apiClient, ILogger<WeatherController> logger) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetWeather()
        {
            logger.LogInformation("Start getting weather");
            var result = await apiClient.GetWeatherDataAsync();
            if (result.StatusCode == 200)
            {
                return Ok($"Weather data: {result.Temperature} degrees");    
            }
            
            logger.LogError("Error getting weather data. Message: {result.Message}", result.Message);
            return StatusCode(result.StatusCode);
            
            
        } 
    }
}