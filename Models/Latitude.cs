using System;
using System.Collections.Generic;

namespace DBFIRST_Cities3.Models;

public partial class Latitude
{
    public int LatitudeId { get; set; }

    public string? Latitude1 { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
