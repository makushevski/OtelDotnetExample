namespace OtelDotnetExample.Api.Client.Models;

public record WeatherResult : BaseResponseModel
{
    public int Temperature { get; init; }
    public int Humidity { get; init; }
}