using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OtelDotnetExample.Api.Client;

namespace OtelDotnetExample.Gateway;

internal static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddSingleton<IOtelDotnetExampleApiClient>(new OtelDotnetExampleApiClient(new Uri("http://localhost:5001")));
        
        builder.WebHost.UseUrls("http://localhost:5000");


        // swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.MapGet("/", () => Results.Redirect("/swagger"))
            .ExcludeFromDescription();

        app.MapControllers();
        app.Run();
    }
}