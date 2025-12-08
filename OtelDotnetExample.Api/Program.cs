using System.Reflection;
using Microsoft.EntityFrameworkCore;
using OtelDotnetExample.Api.DataLayer;
using OtelDotnetExample.Api.DataLayer.Migrations;

namespace OtelDotnetExample.Api
{
    internal static class Program
    {
        public static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddDbContext<PgContext>(optionsBuilder =>
                optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("PgContext"), 
                    contextOptionsBuilder => contextOptionsBuilder.MigrationsAssembly(Assembly.GetAssembly(typeof(InitDbMigration))!)));
            
            //http://localhost:5001/v1/weather
            builder.WebHost.UseUrls("http://localhost:5001");

            var app = builder.Build();
            
            app.MapControllers();
            return app.RunAsync();
        }
    }
}