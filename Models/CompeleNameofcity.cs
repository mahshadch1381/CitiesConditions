using System;
using System.Collections.Generic;

namespace DBFIRST_Cities3.Models;

public partial class CompeleNameofcity
{
    public int CompId { get; set; }

    public string? CopmName { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
