using System;
using System.Collections.Generic;

namespace DBFIRST_Cities3.Models;

public partial class City
{
    public int CityId { get; set; }

    public string? CityName { get; set; }

    public int? HumidityId { get; set; }

    public int? TempId { get; set; }

    public int? CountryId { get; set; }

    public int? CompeleteNameId { get; set; }

    public int? PopulationId { get; set; }

    public int? LongId { get; set; }

    public int? LatId { get; set; }

    public string? Modifitime { get; set; }

    public virtual CompeleNameofcity? CompeleteName { get; set; }

    public virtual Country1? Country { get; set; }

    public virtual Humidity? Humidity { get; set; }

    public virtual Latitude? Lat { get; set; }

    public virtual Longitude? Long { get; set; }

    public virtual Pop? Population { get; set; }

    public virtual Temperature? Temp { get; set; }
}
