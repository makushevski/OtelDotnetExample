using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OtelDotnetExample.Api.Client.Models;

namespace OtelDotnetExample.Api.Client;

public interface IOtelDotnetExampleApiClient
{
    public Task<WeatherModel> GetWeatherDataAsync();
}

public class OtelDotnetExampleApiClient : IOtelDotnetExampleApiClient
{
    private readonly HttpClient _httpClient;

    public OtelDotnetExampleApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public OtelDotnetExampleApiClient(Uri apiBaseUri)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = apiBaseUri;
    }

    public async Task<WeatherModel> GetWeatherDataAsync()
    {
        var c = await _httpClient.GetAsync("/v1/weather").ConfigureAwait(false);
        var readAsStringAsync = await c.Content.ReadAsStringAsync();
        var weatherDataAsync = JsonSerializer.Deserialize<WeatherModel>(readAsStringAsync, JsonSerializerOptions.Web);
        return weatherDataAsync ?? new WeatherModel
        {
            StatusCode = 500,
            Message = "Error getting weather data"
        };
        ;
    }
}