using Microsoft.EntityFrameworkCore;
using OtelDotnetExample.Api.DataLayer.DbModels;

namespace OtelDotnetExample.Api.DataLayer;

public class PgContext(DbContextOptions<PgContext> options) : DbContext(options)
{
    public DbSet<WeatherDbModel> Weather => Set<WeatherDbModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var weather = modelBuilder.Entity<WeatherDbModel>();
        weather.ToTable("weather_data");
        weather.HasKey(x => x.Id);
        weather.Property(x => x.Id).HasColumnName("id");
        weather.Property(x => x.Temperature).HasColumnName("temperature");
        weather.Property(x => x.Humidity).HasColumnName("humidity");
    }
}
