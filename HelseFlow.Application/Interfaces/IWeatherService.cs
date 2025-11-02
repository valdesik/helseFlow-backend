namespace HelseFlow_Backend.Application.Interfaces;

public interface IWeatherService
{
    Task<WeatherDto?> GetCurrentWeatherAsync(string city);
}

public class WeatherDto
{
    public string City { get; set; }
    public string Description { get; set; }
    public double TemperatureCelsius { get; set; }
    public double FeelsLikeCelsius { get; set; }
    public int Humidity { get; set; }
    public double WindSpeedMetersPerSecond { get; set; }
}
