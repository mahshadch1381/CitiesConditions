using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

public interface IInformationService
{
    Task<string> GetCityLatitudeAsync(string city);
    Task<string> GetCityLongitudeAsync(string city);
    
    Task<( string longitude, string latitude)> GetCityInformationAsync(string city);
}

public class RegionService : IInformationService
{
    private readonly HttpClient _httpClient;

    public RegionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    
    public async Task<string> GetCityLongitudeAsync(string city)
    {
        return await GetCityInfo(city, "lon");
    }
     
    public async Task<string> GetCityLatitudeAsync(string city)
    {
        return await GetCityInfo(city, "lat");
    }

    private async Task<string> GetCityInfo(string city, string infoKey)
    {
        string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(city)}&format=json";
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "MyGeocodingApp/1.0");
        HttpResponseMessage response = await _httpClient.GetAsync(url);
        string content = await response.Content.ReadAsStringAsync();
        JArray data = JArray.Parse(content);
        JObject cityInfo = (JObject)data[0];
        return "" + cityInfo[infoKey];
    }

    public async Task<( string longitude, string latitude)> GetCityInformationAsync(string city)
    {
      
        var longitudeTask = GetCityLongitudeAsync(city);
        var latitudeTask = GetCityLatitudeAsync(city);

        await Task.WhenAll( longitudeTask, latitudeTask);

        return ( longitudeTask.Result, latitudeTask.Result);
    }

}