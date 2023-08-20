using System;
using System.Collections.Generic;

namespace DBFIRST_Cities3.Models;

public partial class Country1
{
    public int CountryId { get; set; }

    public string? CountryName { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
