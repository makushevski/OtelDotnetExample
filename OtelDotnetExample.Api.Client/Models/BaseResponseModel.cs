namespace OtelDotnetExample.Api.Client.Models;

public abstract record BaseResponseModel
{
    public int StatusCode { get; init; }
    public string? Message { get; init; }
}