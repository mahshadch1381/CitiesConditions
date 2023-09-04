namespace First_Project.Services
{
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    public interface ICityPopulationService
    {
        Task<int> GetCityPopulationAsync(string city);
        Task<string> GetCitycountryNameAsync(string city);
        Task<(int population, string countryName)> GetCityPopulationAndCountryAsync(string city);
    }
    public class CityPopulationService : ICityPopulationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CityPopulationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            string geoNamesUrl = _configuration["ApiUrls:GeoNames"];
            _httpClient.BaseAddress = new Uri(geoNamesUrl);
        }

        public async Task<int> GetCityPopulationAsync(string city)
        {
            try
            {
                string username = "mahshadch1381";
                string url = $"searchJSON?q={Uri.EscapeDataString(city)}&username={username}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(json);
                    int population = (int)data?.geonames[0]?.population;

                    return population;
                }

                return 100;
            }
            catch (Exception ex)
            {

                return 1;
            }
        }

        public async Task<string> GetCitycountryNameAsync(string city)
        {
            try
            {
                string username = "mahshadch1381";
                string url = $"searchJSON?q={Uri.EscapeDataString(city)}&username={username}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(json);

                    string countryName = (string)data?.geonames[0]?.countryName;

                    return countryName;
                }

                return "l";
            }
            catch (Exception ex)
            {

                return "lp";
            }
        }
        public async Task<(int population, string countryName)> GetCityPopulationAndCountryAsync(string city)
        {
            var populationTask = GetCityPopulationAsync(city);
            var countryNameTask = GetCitycountryNameAsync(city);

            await Task.WhenAll(populationTask, countryNameTask);

            return (populationTask.Result, countryNameTask.Result);
        }
    }
}
