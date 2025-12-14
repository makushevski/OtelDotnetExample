using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OtelDotnetExample.Api.Client;

namespace OtelDotnetExample.Gateway;

internal static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var serviceName = Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME")
                          ?? builder.Configuration["Service:Name"]
                          ?? builder.Environment.ApplicationName;
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(serviceName);

        builder.Services.AddControllers();
        builder.Services.AddHttpClient<IOtelDotnetExampleApiClient, OtelDotnetExampleApiClient>((sp, client) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var apiBase = configuration["Api:BaseAddress"] ?? "http://localhost:5001";
            client.BaseAddress = new Uri(apiBase);
        });

        builder.WebHost.UseUrls(builder.Configuration["ASPNETCORE_URLS"] ?? "http://0.0.0.0:5000");

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddOpenTelemetry()
            .WithTracing(tracerProviderBuilder => tracerProviderBuilder
                .SetResourceBuilder(resourceBuilder)
                .AddSource("Microsoft.AspNetCore")
                .AddSource("System.Net.Http")
                .AddAspNetCoreInstrumentation(options => options.RecordException = true)
                .AddHttpClientInstrumentation()
                .AddOtlpExporter())
            .WithMetrics(meterProviderBuilder => meterProviderBuilder
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter());

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(resourceBuilder);
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
            options.ParseStateValues = true;
            options.AddOtlpExporter();
        });

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.MapGet("/", () => Results.Redirect("/swagger"))
            .ExcludeFromDescription();

        app.MapControllers();
        app.Run();
    }
}
