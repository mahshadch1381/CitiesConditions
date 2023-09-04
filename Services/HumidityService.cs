namespace First_Project.Services
{
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    public interface IHumidityService
    {

        Task<int> GetCityHumidityAsync(string city);

    }
    public class HumidityService : IHumidityService
    {
        private readonly HttpClient _httpClient;
    
        private readonly IConfiguration _configuration;

        public HumidityService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            string huUrl = _configuration["ApiUrls:humidity"];
            _httpClient.BaseAddress = new Uri(huUrl);
          
        }

        public async Task<int> GetCityHumidityAsync(string city)
        {
            try
            {
                string openWeatherMapBaseUrl = _configuration["ApiUrls:humidity"];
                string apiKey = _configuration["ApiKeys:OpenWeatherMapkey"];
                string escapedCity = Uri.EscapeDataString(city);
                string url = $"{openWeatherMapBaseUrl}/weather?q={escapedCity}&appid={apiKey}&units=metric"; 
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(json);
                    int Humidity = (int)data?.main?.humidity;
                    

                    return Humidity;
                }

                return 0;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return 1;
            }
        }


    }


}
