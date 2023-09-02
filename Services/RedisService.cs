using DBFIRST_Cities3.DTO;
using DBFIRST_Cities3.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DBFIRST_Cities3.Services
{
   
    public interface IRedisService
    {
        void StoreCityInRedisWithExpiry(CityDto city);
        CityDto GetCityFromRedisByName(string cityName);
        bool ContainsCityInRedisByName(string cityName);
        bool DeleteCityInRedisByName(string cityName);
        bool UpdateCityInRedis(CityDto updatedCity);
    }
    public class RedisService : IRedisService
    {
        private readonly ConnectionMultiplexer _connection;

        public RedisService(IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("Redis");
            _connection = ConnectionMultiplexer.Connect(redisConnectionString);
        }

        public void StoreCityInRedisWithExpiry(CityDto city)
        {
            var db = _connection.GetDatabase();
            var key = $"City:{city.Name}";
            var value = JsonConvert.SerializeObject(city);
            var expiry = TimeSpan.FromMinutes(5);
            db.StringSet(key, value, expiry);
        }

        public CityDto GetCityFromRedisByName(string cityName)
        {
            var db = _connection.GetDatabase();
            var key = $"City:{cityName}";
            var cityJson = db.StringGet(key);
            if (!cityJson.IsNull)
            {
                var city = JsonConvert.DeserializeObject<CityDto>(cityJson);
                return city;
            }

            return null;
        }

        public bool ContainsCityInRedisByName(string cityName)
        {
            var db = _connection.GetDatabase();
            var key = $"City:{cityName}";
            return db.KeyExists(key);
        }
        public bool DeleteCityInRedisByName(string cityName)
        {
            var db = _connection.GetDatabase();
            var key = $"City:{cityName}";

            if (db.KeyDelete(key))
            {
                return true; 
            }

            return false; 
        }
        public bool UpdateCityInRedis(CityDto updatedCity)
        {
            var db = _connection.GetDatabase();
            var key = $"City:{updatedCity.Name}";

            if (db.KeyExists(key))
            {
                var value = JsonConvert.SerializeObject(updatedCity);
                var expiry = TimeSpan.FromMinutes(5);
                db.StringSet(key, value, expiry);
                return true; 
            }

            return false; 
        }
    }

}
