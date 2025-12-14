using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OtelDotnetExample.Api.Client;

namespace OtelDotnetExample.Gateway.Controllers;

[Route("v1/weather")]
public class WeatherController(IOtelDotnetExampleApiClient apiClient, ILogger<WeatherController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetWeather(
        [FromQuery] bool throwException = false,
        [FromQuery] int delayMs = 0,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Start getting weather (throwException={ThrowException}, delayMs={DelayMs})",
            throwException, delayMs);

        var result = await apiClient.GetWeatherDataAsync(throwException, delayMs, cancellationToken);
        if (result.StatusCode == StatusCodes.Status200OK)
        {
            return Ok($"Weather data: {result.Temperature} degrees, humidity {result.Humidity}%");
        }

        logger.LogError("Error getting weather data. Status: {StatusCode}. Message: {Message}",
            result.StatusCode, result.Message);
        return StatusCode(result.StatusCode, result.Message);
    }
}
