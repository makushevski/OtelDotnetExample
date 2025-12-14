using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OtelDotnetExample.Api.Client.Models;

namespace OtelDotnetExample.Api.Client;

public interface IOtelDotnetExampleApiClient
{
    Task<WeatherModel> GetWeatherDataAsync(bool throwException = false, int delayMs = 0, CancellationToken cancellationToken = default);
}

public class OtelDotnetExampleApiClient : IOtelDotnetExampleApiClient
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient _httpClient;

    public OtelDotnetExampleApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public OtelDotnetExampleApiClient(Uri apiBaseUri)
    {
        _httpClient = new HttpClient { BaseAddress = apiBaseUri };
    }

    public async Task<WeatherModel> GetWeatherDataAsync(bool throwException = false, int delayMs = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = $"/v1/weather?throwException={throwException.ToString().ToLowerInvariant()}&delayMs={delayMs}";
            var response = await _httpClient.GetAsync(query, cancellationToken).ConfigureAwait(false);
            var payload = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                return new WeatherModel
                {
                    StatusCode = (int)response.StatusCode,
                    Message = string.IsNullOrWhiteSpace(payload) ? response.ReasonPhrase : payload
                };
            }

            var weather = JsonSerializer.Deserialize<WeatherModel>(payload, SerializerOptions);
            return (weather ?? new WeatherModel { Message = "Empty response from API" }) with
            {
                StatusCode = (int)response.StatusCode
            };
        }
        catch (Exception ex)
        {
            return new WeatherModel
            {
                StatusCode = 500,
                Message = ex.Message
            };
        }
    }
}
