using System.Reflection;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OtelDotnetExample.Api.DataLayer;
using OtelDotnetExample.Api.DataLayer.Migrations;

namespace OtelDotnetExample.Api;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var serviceName = Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME")
                          ?? builder.Configuration["Service:Name"]
                          ?? builder.Environment.ApplicationName;
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(serviceName);

        builder.Services.AddControllers();
        builder.Services.AddDbContext<PgContext>(optionsBuilder =>
        {
            var connectionString = builder.Configuration.GetConnectionString("PgContext");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string 'PgContext' is not configured");
            }
            optionsBuilder.UseNpgsql(connectionString,
                contextOptionsBuilder =>
                    contextOptionsBuilder.MigrationsAssembly(typeof(InitDbMigration).Assembly.FullName));
        });

        builder.WebHost.UseUrls(builder.Configuration["ASPNETCORE_URLS"] ?? "http://0.0.0.0:5001");

        builder.Services.AddOpenTelemetry()
            .WithTracing(tracerProviderBuilder => tracerProviderBuilder
                .SetResourceBuilder(resourceBuilder)
                .AddSource("Microsoft.AspNetCore")
                .AddSource("System.Net.Http")
                .AddSource("OtelDotnetExample.Api")
                .AddAspNetCoreInstrumentation(options => options.RecordException = true)
                .AddHttpClientInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()
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

        await ApplyMigrationsAsync(app);

        app.MapControllers();
        await app.RunAsync();
    }

    private static async Task ApplyMigrationsAsync(WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<PgContext>();
        await context.Database.MigrateAsync();
        if (!await context.Weather.AnyAsync())
        {
            context.Weather.Add(new DataLayer.DbModels.WeatherDbModel
            {
                Id = Guid.Parse("0c5c7a28-8c76-4f40-84e4-1a1c0b9b9e3f"),
                Temperature = 20,
                Humidity = 80,
            });
            await context.SaveChangesAsync();
        }
    }
}
