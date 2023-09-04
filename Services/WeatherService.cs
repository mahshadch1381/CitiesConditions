namespace First_Project.Services
{
    using First_Project.relation;
    using System;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    public interface IWeatherService
    {
        Task<double> GetTemperatureAsync(string city);
    }

    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly IConfiguration _configuration;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration; 
            _apiKey = _configuration["ApiKeys:OpenWeatherMapkey"];
             string openWeatherMapBaseUrl = _configuration["ApiUrls:OpenWeatherMap"];
             _httpClient.BaseAddress = new Uri(openWeatherMapBaseUrl);
               
        }
        public async Task<double> GetTemperatureAsync(string city)
        {
            string escapedCity = Uri.EscapeDataString(city);
            string openWeatherMapBaseUrl = _configuration["ApiUrls:OpenWeatherMap"];
            string apiUrl = $"{openWeatherMapBaseUrl}/weather?q={escapedCity}&appid={_apiKey}";
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    WeatherData weatherData = JsonSerializer.Deserialize<WeatherData>(json);
                    if (weatherData != null)
                    {
                        double temperatureCelsius = weatherData.main.temp - 273.15;
                        return temperatureCelsius;
                    }
                    else
                        return 0.0;
                }
                else
                {
                    throw new HttpRequestException($"Failed to fetch weather data. Status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException($"Error: {e.Message}");
            }
        }
    }

}
