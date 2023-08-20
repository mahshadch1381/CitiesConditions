using System;
using System.Collections.Generic;

namespace DBFIRST_Cities3.Models;

public partial class Longitude
{
    public int LongitudeId { get; set; }

    public string? Longitude1 { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
