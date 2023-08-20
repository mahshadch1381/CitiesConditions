namespace DBFIRST_Cities3.DTO
{
    public class CityDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? population { get; set; }
        public string? country { get; set; }
        public double? tempData { get; set; }
        public string? modifiedtime { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        
        public int? Humidity { get; set; }
    }
}
