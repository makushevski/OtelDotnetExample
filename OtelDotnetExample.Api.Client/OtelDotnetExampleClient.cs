using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OtelDotnetExample.Api.Client.Models;

namespace OtelDotnetExample.Api.Client;

public class OtelDotnetExampleClient
{
    private readonly HttpClient _httpClient;

    public OtelDotnetExampleClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public OtelDotnetExampleClient(Uri apiBaseUri)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = apiBaseUri;
    }

    public async Task<WeatherResult> GetWeatherDataAsync()
    {
        var c = await _httpClient.GetAsync("/v1/weather").ConfigureAwait(false);
        var readAsStringAsync = await c.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<WeatherResult>(readAsStringAsync) ?? new WeatherResult
        {
            StatusCode = 500,
            Message = "Error getting weather data"
        };
        ;
    }
}