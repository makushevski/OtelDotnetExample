using System.ComponentModel.DataAnnotations.Schema;

namespace OtelDotnetExample.Api.DataLayer.DbModels;

[Table("weather-data")]
public class WeatherDbModel
{
    public Guid Id { get; set; }
    public int Temperature { get; set; }
    public int Humidity { get; set; }
}