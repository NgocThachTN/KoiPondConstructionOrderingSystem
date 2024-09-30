using System;
using System.Collections.Generic;

namespace KoiPondConstructionManagement.Model;

public partial class ConstructionType
{
    public int ConstructionTypeId { get; set; }

    public string? ConstructionName { get; set; }

    public virtual ICollection<Design> Designs { get; set; } = new List<Design>();

    public virtual ICollection<Sample> Samples { get; set; } = new List<Sample>();
}
