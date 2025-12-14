using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OtelDotnetExample.Api.DataLayer;
using OtelDotnetExample.Api.Models;

namespace OtelDotnetExample.Api.Controllers;

[Route("v1/weather")]
public class WeatherController(PgContext dbContext, ILogger<WeatherController> logger) : ControllerBase
{
    private static readonly ActivitySource ActivitySource = new("OtelDotnetExample.Api");

    [HttpGet]
    public async Task<IActionResult> GetWeather(
        [FromQuery] bool throwException = false,
        [FromQuery] int delayMs = 0,
        CancellationToken cancellationToken = default)
    {
        using var activity = ActivitySource.StartActivity("WeatherController.GetWeather");
        activity?.SetTag("app.throwException", throwException);
        activity?.SetTag("app.delayMs", delayMs);

        if (throwException)
        {
            throw new InvalidOperationException("Requested to throw an exception for testing");
        }

        if (delayMs > 0)
        {
            await Task.Delay(delayMs, cancellationToken);
        }

        var first = await dbContext.Weather.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        if (first is null)
        {
            logger.LogWarning("Weather data is empty");
            return NotFound("Weather data is not initialized");
        }

        logger.LogInformation("Weather data retrieved: {@Weather}", first);
        return Ok(new WeatherResponse
        {
            Temperature = first.Temperature,
            Humidity = first.Humidity,
        });
    }
}
