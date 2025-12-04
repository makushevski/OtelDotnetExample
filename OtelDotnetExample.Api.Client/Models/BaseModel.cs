namespace OtelDotnetExample.Api.Client.Models;

public abstract record BaseModel
{
    public int StatusCode { get; init; } = 200;
    public string? Message { get; init; }
}