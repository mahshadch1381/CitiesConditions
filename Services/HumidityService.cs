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
        private readonly string _apiKey;

        public HumidityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://api.openweathermap.org/data/2.5/");
            _apiKey = "3bd430ec23ca470e35dc0b05c1f50b47";
        }

        public async Task<int> GetCityHumidityAsync(string city)
        {
            try
            {
                string url = $"http://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(city)}&appid={_apiKey}&units=metric";
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
