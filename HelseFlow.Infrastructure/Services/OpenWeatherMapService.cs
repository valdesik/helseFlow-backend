using HelseFlow_Backend.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HelseFlow_Backend.Infrastructure.Services;

public class OpenWeatherMapService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly string _apiKey;

    public OpenWeatherMapService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _apiKey = _configuration["OpenWeatherMap:ApiKey"] ?? throw new InvalidOperationException("OpenWeatherMap API Key not configured.");
        _httpClient.BaseAddress = new Uri("http://api.openweathermap.org/data/2.5/");
    }

    public async Task<WeatherDto?> GetCurrentWeatherAsync(string city)
    {
        var response = await _httpClient.GetAsync($"weather?q={city}&appid={_apiKey}&units=metric");
        response.EnsureSuccessStatusCode(); // Throws an exception if the HTTP response status is an error code

        var jsonString = await response.Content.ReadAsStringAsync();
        var weatherData = JsonSerializer.Deserialize<OpenWeatherMapResponse>(jsonString);

        if (weatherData == null || weatherData.main == null || weatherData.weather == null || !weatherData.weather.Any())
        {
            return null;
        }

        return new WeatherDto
        {
            City = weatherData.name,
            Description = weatherData.weather.First().description,
            TemperatureCelsius = weatherData.main.temp,
            FeelsLikeCelsius = weatherData.main.feels_like,
            Humidity = weatherData.main.humidity,
            WindSpeedMetersPerSecond = weatherData.wind.speed
        };
    }

    // Internal classes to deserialize OpenWeatherMap API response
    private class OpenWeatherMapResponse
    {
        public MainData main { get; set; } = new MainData();
        public WeatherDetail[] weather { get; set; } = Array.Empty<WeatherDetail>();
        public WindData wind { get; set; } = new WindData();
        public string name { get; set; } = string.Empty;
    }

    private class MainData
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public int humidity { get; set; }
    }

    private class WeatherDetail
    {
        public string description { get; set; } = string.Empty;
    }

    private class WindData
    {
        public double speed { get; set; }
    }
}
