namespace OtelDotnetExample.Api.DataLayer.DbModels;

public class WeatherDbModel
{
    public Guid Id { get; set; }
    public int Temperature { get; set; }
    public int Humidity { get; set; }
}
