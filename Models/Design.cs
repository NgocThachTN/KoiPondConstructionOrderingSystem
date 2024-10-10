using System;
using System.Collections.Generic;

namespace KoiPond.Models;

public partial class Design
{
    public int DesignId { get; set; }

    public int? ConstructionTypeId { get; set; }

    public string? DesignName { get; set; }

    public string? DesignSize { get; set; }

    public double? DesignPrice { get; set; }

    public string? DesignImage { get; set; }

    public virtual ConstructionType? ConstructionType { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
