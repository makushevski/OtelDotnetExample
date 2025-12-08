using Microsoft.EntityFrameworkCore;
using OtelDotnetExample.Api.DataLayer.DbModels;

namespace OtelDotnetExample.Api.DataLayer;

public class PgContext : DbContext
{
    public DbSet<WeatherDbModel> Weather { get; }
    
    public PgContext(DbContextOptions<PgContext> options)
    {
        
    }
}