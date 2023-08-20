using DBFIRST_Cities3.DTO;
using DBFIRST_Cities3.Models;
using First_Project.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DBFIRST_Cities3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/<ValuesController>
        private readonly WorldContext _Context;
        private readonly IWeatherService _weatherService;
        private readonly IInformationService _informationService;
        private readonly ICityPopulationService _icitypopulation;
        private readonly IHumidityService _huservice;

        public ValuesController(WorldContext dbContext, IWeatherService weatherService, IInformationService informationService,
            ICityPopulationService cityPopulation, IHumidityService humidityservice)
        {
            _Context = dbContext;
            _weatherService = weatherService;
            _informationService = informationService;
            _icitypopulation = cityPopulation;
            _huservice = humidityservice;
        }
        
        [HttpPost]
        public async Task<ActionResult<List<RecommandCities>>> Post([FromBody] InputCondition ic)
        {
            List<int> crowded = new List<int>();
            List<int> country = new List<int>();
            List<int> weather = new List<int>();
            List<int> ns = new List<int>();
            List<int> ew = new List<int>();
            List<int> hum = new List<int>();

            if (ic.NOmatter2_crowded_YES1_NO0 == 0){
                crowded = (from city in _Context.Cities
                        join Pop in _Context.Pops on city.PopulationId equals Pop.PopId
                        where Pop.Popvalue < 900000   // Replace with your condition
                        select city.CityId).ToList();
            }
            if (ic.NOmatter2_crowded_YES1_NO0 == 1)
            {
                crowded = (from city in _Context.Cities
                          join Pop in _Context.Pops on city.PopulationId equals Pop.PopId
                          where Pop.Popvalue >= 900000   // Replace with your condition
                          select city.CityId).ToList();
            }
            if (ic.NOmatter2_crowded_YES1_NO0 == 2)
            {
                crowded = (from city in _Context.Cities
                          join Pop in _Context.Pops on city.PopulationId equals Pop.PopId
                          select city.CityId).ToList();
            }
            if (ic.NOmatter_country == null)
            {
                country = (from city in _Context.Cities
                         join Counry in _Context.Country1s on city.CountryId equals Counry.CountryId
                         select city.CityId).ToList();
            }
            if (ic.NOmatter_country != null)
            {
                country = (from city in _Context.Cities
                         join Counry in _Context.Country1s on city.CountryId equals Counry.CountryId
                         where Counry.CountryName == ic.NOmatter_country   // Replace with your condition
                         select city.CityId).ToList();
            }
            if (ic.NOmatter0_Warm1_Cold2_Medium3 == 0)
            {
                weather = (from city in _Context.Cities
                          join Temperature in _Context.Temperatures on city.TempId equals Temperature.TempId
                          select city.CityId).ToList();
            }
            if (ic.NOmatter0_Warm1_Cold2_Medium3 == 1)
            {
                weather = (from city in _Context.Cities
                          join Temperature in _Context.Temperatures on city.TempId equals Temperature.TempId
                          where Temperature.Temperature1 >30 // Replace with your condition
                          select city.CityId).ToList();
            }
            if (ic.NOmatter0_Warm1_Cold2_Medium3 == 2)
            {
                weather = (from city in _Context.Cities
                          join Temperature in _Context.Temperatures on city.TempId equals Temperature.TempId
                          where Temperature.Temperature1 <= 20 // Replace with your condition
                          select city.CityId).ToList();
            }
            if (ic.NOmatter0_Warm1_Cold2_Medium3 == 3)
            {
                weather = (from city in _Context.Cities
                          join Temperature in _Context.Temperatures on city.TempId equals Temperature.TempId
                          where Temperature.Temperature1 < 30 && Temperature.Temperature1 > 20 // Replace with your condition
                          select city.CityId).ToList();
            }

            if (ic.NOmatter2_South1_North0 == 0)
            {
                ns = (from city in _Context.Cities
                          join Latitude in _Context.Latitudes on city.LatId equals Latitude.LatitudeId
                          where Convert.ToDouble(Latitude.Latitude1) >0 // Replace with your condition
                          select city.CityId).ToList();
            }
            if (ic.NOmatter2_South1_North0 == 1)
            {
                ns = (from city in _Context.Cities
                     join Latitude in _Context.Latitudes on city.LatId equals Latitude.LatitudeId
                     where Convert.ToDouble(Latitude.Latitude1) < 0 // Replace with your condition
                     select city.CityId).ToList();
            }
            if (ic.NOmatter2_South1_North0 == 2)
            {
                ns = (from city in _Context.Cities
                     join Latitude in _Context.Latitudes on city.LatId equals Latitude.LatitudeId
                     select city.CityId).ToList();
            }
            if (ic.NOmatter2_East1_West0 == 1)
            {
                ew = (from city in _Context.Cities
                     join Longitude in _Context.Longitudes on city.LongId equals Longitude.LongitudeId
                     where Convert.ToDouble(Longitude.Longitude1) > 0 // Replace with your condition
                     select city.CityId).ToList();
            }
            if (ic.NOmatter2_East1_West0 == 1)
            {
                ew = (from city in _Context.Cities
                     join Longitude in _Context.Longitudes on city.LongId equals Longitude.LongitudeId
                     where Convert.ToDouble(Longitude.Longitude1) < 0 // Replace with your condition
                     select city.CityId).ToList();
            }
            if (ic.NOmatter2_East1_West0 == 2)
            {
                ew = (from city in _Context.Cities
                     join Longitude in _Context.Longitudes on city.LongId equals Longitude.LongitudeId
                     select city.CityId).ToList();
            }
            if (ic.HumidityOK_yes1_NO0 == 1)
            {
                hum = (from city in _Context.Cities
                      join Humidity in _Context.Humiditys on city.HumidityId equals Humidity.Humidityid
                      select city.CityId).ToList();
            }
            if (ic.HumidityOK_yes1_NO0 == 0)
            {
                hum = (from city in _Context.Cities
                     join Humidity in _Context.Humiditys on city.HumidityId equals Humidity.Humidityid
                     where Humidity.Humidity1 < 50 // Replace with your condition
                     select city.CityId).ToList();
            }
            var combinedResults = from cityId in crowded
                                  join countryId in country on cityId equals countryId
                                  join weatherId in weather on cityId equals weatherId
                                  join nsId in ns on cityId equals nsId
                                  join ewId in ew on cityId equals ewId
                                  join humId in hum on cityId equals humId
                                  select cityId;

            List<string> finalResult = (from city in _Context.Cities
                              where combinedResults.Contains(city.CityId)
                              select city.CityName).ToList();
            return Ok(finalResult);
        }
        

       

    }
}
