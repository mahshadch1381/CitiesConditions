using System;
using System.Collections.Generic;

namespace DBFIRST_Cities3.Models;

public partial class Humidity
{
    public int Humidityid { get; set; }

    public int? Humidity1 { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
