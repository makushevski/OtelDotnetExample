namespace OtelDotnetExample.Api.Client.Models;

public record WeatherModel : BaseModel
{
    public int Temperature { get; init; }
    public int Humidity { get; init; }
}