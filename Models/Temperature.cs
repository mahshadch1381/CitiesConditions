using System;
using System.Collections.Generic;

namespace DBFIRST_Cities3.Models;

public partial class Temperature
{
    public int TempId { get; set; }

    public int? Temperature1 { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
