using System;
using System.Collections.Generic;

namespace DBFIRST_Cities3.Models;

public partial class Pop
{
    public int PopId { get; set; }

    public int? Popvalue { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
