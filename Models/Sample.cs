using System;
using System.Collections.Generic;

namespace KoiPond.Models;

public partial class Sample
{
    public int SampleId { get; set; }

    public int? ConstructionTypeId { get; set; }

    public string? SampleName { get; set; }

    public string? SampleSize { get; set; }

    public double? SamplePrice { get; set; }

    public string? SampleImage { get; set; }

    public virtual ConstructionType? ConstructionType { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
