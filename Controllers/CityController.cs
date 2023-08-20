using DBFIRST_Cities3.DTO;
using DBFIRST_Cities3.Models;
using First_Project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Db_CitiesProject2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly WorldContext _Context;
        private readonly IWeatherService _weatherService;
        private readonly IInformationService _informationService;
        private readonly ICityPopulationService _icitypopulation;
        private readonly IHumidityService _huservice;

        public CityController(WorldContext dbContext,IWeatherService weatherService, IInformationService informationService,
            ICityPopulationService cityPopulation, IHumidityService humidityservice)
        {
            _Context = dbContext;
            _weatherService = weatherService;
            _informationService = informationService;
            _icitypopulation = cityPopulation;
            _huservice = humidityservice;
        }


        [HttpGet("GetAllCities")]
        public async Task<ActionResult<List<City>>> GetAllCities()
        {
            List<CityDto> cityList = new List<CityDto>();
            var cities = await _Context.Cities.ToListAsync();
            foreach(City city in cities)
            {
                var temperature1 = _Context.Temperatures
                                 .Where(temp => temp.TempId == city.TempId)
                                  .Select(temp => temp.Temperature1)
                                   .SingleOrDefault();
                var Lat1 = _Context.Latitudes
                          .Where(temp => temp.LatitudeId == city.LatId)
                          .Select(temp => temp.Latitude1)
                          .SingleOrDefault();
                var Log1 = _Context.Longitudes
                         .Where(temp => temp.LongitudeId == city.LongId)
                         .Select(temp => temp.Longitude1)
                         .SingleOrDefault();
                var COUNTRYNAME = _Context.Country1s
                         .Where(temp => temp.CountryId == city.CountryId)
                         .Select(temp => temp.CountryName)
                         .SingleOrDefault();
                var POPULATION = _Context.Pops
                         .Where(temp => temp.PopId == city.PopulationId)
                         .Select(temp => temp.Popvalue)
                         .SingleOrDefault();
                var humidity = _Context.Humiditys
                        .Where(temp => temp.Humidityid == city.HumidityId)
                        .Select(temp => temp.Humidity1)
                        .SingleOrDefault();
                CityDto cd = new CityDto();
                cd.population = POPULATION;
                cd.modifiedtime = city.Modifitime;
                cd.Name = city.CityName;
                cd.country = COUNTRYNAME;
                cd.Latitude = Lat1;
                cd.Longitude = Log1;
                cd.Humidity = humidity;
                cd.tempData = temperature1;
                cd.Id = city.CityId;
                cityList.Add(cd);
            }    
           
            return Ok(cityList);
        }
        // GET: api/<CitiesController>
        [HttpGet("GetTempOfCity")]
        public async Task<ActionResult<City>> Get(string cityname)
        {
            try
            {
                City b = await _Context.Cities.FirstOrDefaultAsync(c => c.CityName == cityname);
                if (b == null)
                {
                    
                    City city = new City();
                    city.CityName = cityname;
                    DateTime dateTime = DateTime.Now;
                    city.Modifitime = dateTime.ToString();
                    
                    try
                    {
                        double temperature = await _weatherService.GetTemperatureAsync(city.CityName);
                        string formattedNumber = temperature.ToString("0.00");
                        double temp = Double.Parse(formattedNumber);
                        Temperature t = new Temperature();
                        t.Temperature1 = Convert.ToInt32(temp);
                        t.Cities = new List<City>();    
                        _Context.Temperatures.Add(t);
                        city.Temp = t;
                        await _Context.SaveChangesAsync();
                        var g = await _Context.Temperatures.FirstOrDefaultAsync(c => c.Temperature1 == Convert.ToInt32(temp));
                        city.TempId = g?.TempId;
                        await _Context.SaveChangesAsync();
                    }
                    catch (HttpRequestException ex)
                    {
                        if (ex.InnerException != null)
                        {
                            var innerException = ex.InnerException;
                        }
                        return StatusCode(500, $"Error fetching weather data: {ex.Message}");
                    }
                    try
                    {
                        var ( lg, lt) = await _informationService.GetCityInformationAsync(cityname);
                        

                        Longitude longit = new Longitude();
                        longit.Cities= new List<City>();
                        longit.Longitude1 = lg;
                        city.Long= longit;  
                        _Context.Longitudes.Add(longit);    

                        Latitude latitude = new Latitude();
                        latitude.Latitude1 = lt;
                        latitude.Cities = new List<City>();
                        city.Lat= latitude; 
                        _Context.Latitudes.Add(latitude);
                        await _Context.SaveChangesAsync();


                       

                        var g1 = await _Context.Longitudes.FirstOrDefaultAsync(c => c.Longitude1 == lg);
                        city.LongId = g1?.LongitudeId;

                        var g2 = await _Context.Latitudes.FirstOrDefaultAsync(c => c.Latitude1 == lt);
                        city.LatId = g2?.LatitudeId;
                        await _Context.SaveChangesAsync();

                    }
                    catch (HttpRequestException ex)
                    {
                        if (ex.InnerException != null)
                        {
                            var innerException = ex.InnerException;
                        }
                        return StatusCode(500, $"Error fetching weather data: {ex.Message}");
                    }
                    try
                    {
                        var (pop, countryNm) = await _icitypopulation.GetCityPopulationAndCountryAsync(cityname);
                        Pop p = new Pop();  
                        p.Cities = new List<City>();
                        p.Popvalue = pop;
                        city.Population = p;
                        _Context.Pops.Add(p);


                        Country1 country = new Country1();
                        country.Cities = new List<City>();
                        country.CountryName = countryNm;
                        city.Country= country;  
                        _Context.Country1s.Add(country);
                        await _Context.SaveChangesAsync();

                        var g1 = await _Context.Pops.FirstOrDefaultAsync(c => c.Popvalue == pop);
                        city.PopulationId = g1?.PopId;

                        var g2 = await _Context.Country1s.FirstOrDefaultAsync(c => c.CountryName == countryNm);
                        city.CountryId = g2?.CountryId;
                        
                    }
                    catch (HttpRequestException ex)
                    {
                        if (ex.InnerException != null)
                        {
                            var innerException = ex.InnerException;
                        }
                        return StatusCode(500, $"Error fetching weather data: {ex.Message}");
                    }
                    try
                    {
                       int str = await _huservice.GetCityHumidityAsync(cityname);
                       Humidity humidity = new Humidity();
                       humidity.Cities = new List<City>();
                       humidity.Humidity1 = str;
                       _Context.Humiditys.Add(humidity);
                        await _Context.SaveChangesAsync();
                        city.Humidity = humidity;
                        var g2 = await _Context.Humiditys.FirstOrDefaultAsync(c => c.Humidity1 == str);
                        city.HumidityId = g2?.Humidityid;
                        
                    }
                    catch (HttpRequestException ex)
                    {
                        if (ex.InnerException != null)
                        {
                            var innerException = ex.InnerException;
                        }
                        return StatusCode(500, $"Error fetching weather data: {ex.Message}");
                    }
                   


                    await _Context.SaveChangesAsync();


                    city.Humidity.Cities.Add(city);
                    city.Lat.Cities.Add(city);
                    city.Long.Cities.Add(city);
                    city.Population.Cities.Add(city);
                    city.Country.Cities.Add(city); 
                    city.Temp.Cities.Add(city);

                    await _Context.SaveChangesAsync();

                    string modifyTime = $"Last updated Time is {city.Modifitime}";
                    string apiResponse = $"Temperature in {city.CityName} : {city.Temp.Temperature1}°C";
                    string moreinfo = $"Latitude  : {city.Lat.Latitude1},\nLongitude : {city.Long.Longitude1} ,";
                    string popp = $"countryname  : {city.Country.CountryName},\npopulation : {city.Population.Popvalue}";
                    string hum = $"Humidity : {city.Humidity.Humidity1}";
                    return Ok(apiResponse + ".\n" + modifyTime + ".\n" + moreinfo + "\n" + popp + "\n" + hum);
                }
                
                DateTime submissionTime = DateTime.Parse(b.Modifitime);

                DateTime currentTime = DateTime.Now;
                TimeSpan timeDifference = currentTime - submissionTime;
                
                if (timeDifference.TotalMinutes >= 30)
                {
                    double temperature = await _weatherService.GetTemperatureAsync(b.CityName);
                    DateTime dateTime = DateTime.Now;
                    b.Modifitime = dateTime.ToString();
                    
                   string formattedNumber = temperature.ToString("0.00");
                    double temp = Double.Parse(formattedNumber);
                    int a = (int)temp;
                    if (b.Temp != null)
                    {
                        b.Temp.Temperature1 = a;
                    }
                    var g2 = await _Context.Temperatures.FirstOrDefaultAsync(c => c.TempId == b.TempId);
                    if (g2 != null)
                    {
                        g2.Temperature1 = a;
                    }

                    int str = await _huservice.GetCityHumidityAsync(b.CityName);
                    if (b.Humidity != null)
                    {
                        b.Humidity.Humidity1 = str;
                    }
                    var g3 = await _Context.Humiditys.FirstOrDefaultAsync(c => c.Humidityid == b.HumidityId);
                    if (g3 != null)
                    {
                       g3.Humidity1 = a;
                    }
                    await _Context.SaveChangesAsync();

                    var temperature1 = _Context.Temperatures
                             .Where(temp => temp.TempId == b.TempId)
                             .Select(temp => temp.Temperature1)
                             .SingleOrDefault();
                    var Lat1 = _Context.Latitudes
                              .Where(temp => temp.LatitudeId == b.LatId)
                              .Select(temp => temp.Latitude1)
                              .SingleOrDefault();
                    var Log1 = _Context.Longitudes
                             .Where(temp => temp.LongitudeId == b.LongId)
                             .Select(temp => temp.Longitude1)
                             .SingleOrDefault();
                    var COUNTRYNAME = _Context.Country1s
                             .Where(temp => temp.CountryId == b.CountryId)
                             .Select(temp => temp.CountryName)
                             .SingleOrDefault();
                    var POPULATION = _Context.Pops
                             .Where(temp => temp.PopId == b.PopulationId)
                             .Select(temp => temp.Popvalue)
                             .SingleOrDefault();
                    var humidity = _Context.Humiditys
                            .Where(temp => temp.Humidityid == b.HumidityId)
                            .Select(temp => temp.Humidity1)
                            .SingleOrDefault();
                    string modifyTime = $"Last updated Time is {b.Modifitime}";
                    string apiResponse = $"Temperature in {b.CityName} : {temperature1}°C";
                    string moreinfo = $"Latitude  : {Lat1},\nLongitude : {Log1} ,";
                    string popp = $"countryname  : {COUNTRYNAME},\npopulation : {POPULATION}";
                    string hum = $"Humidity : {humidity}";
                    return Ok("update" + "\n" + apiResponse + ".\n" + modifyTime + ".\n" + moreinfo + "\n" + popp + "\n" + hum);
                }
                else
                {
             
                    var temperature = _Context.Temperatures
                             .Where(temp => temp.TempId == b.TempId)
                             .Select(temp => temp.Temperature1)
                             .SingleOrDefault();
                    var Lat1 = _Context.Latitudes
                              .Where(temp => temp.LatitudeId == b.LatId)
                              .Select(temp => temp.Latitude1)
                              .SingleOrDefault();
                    var Log1 = _Context.Longitudes
                             .Where(temp => temp.LongitudeId == b.LongId)
                             .Select(temp => temp.Longitude1)
                             .SingleOrDefault();
                    var COUNTRYNAME = _Context.Country1s
                             .Where(temp => temp.CountryId == b.CountryId)
                             .Select(temp => temp.CountryName)
                             .SingleOrDefault();
                    var POPULATION = _Context.Pops
                             .Where(temp => temp.PopId == b.PopulationId)
                             .Select(temp => temp.Popvalue)
                             .SingleOrDefault();
                    var humidity = _Context.Humiditys
                            .Where(temp => temp.Humidityid == b.HumidityId)
                            .Select(temp => temp.Humidity1)
                            .SingleOrDefault();
                    string modifyTime = $"Last updated Time is {b.Modifitime}";
                    string apiResponse = $"Temperature in {b.CityName} : {temperature}°C";
                    string moreinfo = $"Latitude  : {Lat1},\nLongitude : {Log1} ,";
                    string popp = $"countryname  : {COUNTRYNAME},\npopulation : {POPULATION}";
                    string hum = $"Humidity : {humidity}";
                    return Ok(apiResponse + ".\n" + modifyTime + ".\n" + moreinfo + "\n" + popp + "\n" + hum);
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null)
                {
                    var innerException = ex.InnerException;
                }
                return StatusCode(500, $"Error fetching weather data: {ex.Message}");
            }

        }

        // DELETE api/<CitiesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
