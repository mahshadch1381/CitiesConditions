using DBFIRST_Cities3.DTO;
using DBFIRST_Cities3.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DBFIRST_Cities3.Services
{
    public class MyRedisService
    {
        private static Lazy<ConnectionMultiplexer> _lazyConnection;

        static MyRedisService()
        {
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                var redisConnectionString = configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(redisConnectionString);
            });
        }

        public static ConnectionMultiplexer Connection => _lazyConnection.Value;

        public static void StoreCityInRedisWithExpiry(CityDto city)
        {
            var db = Connection.GetDatabase();
            var key = $"City:{city.Name}";
            var value = JsonConvert.SerializeObject(city);
            var expiry = TimeSpan.FromMinutes(5);
            db.StringSet(key, value, expiry);
        }

        public static CityDto GetCityFromRedisByName(string cityName)
        {
            var db = Connection.GetDatabase();
            var key = $"City:{cityName}";
            var cityJson = db.StringGet(key);
            if (!cityJson.IsNull)
            {
                var city = JsonConvert.DeserializeObject<CityDto>(cityJson);
                return city;
            }

            return null;
        }

        public static bool ContainsCityInRedisByName(string cityName)
        {
            var db = Connection.GetDatabase();
            var key = $"City:{cityName}";
            return db.KeyExists(key);
        }
    }
}
