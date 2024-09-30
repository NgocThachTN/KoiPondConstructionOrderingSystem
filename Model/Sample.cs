using System;
using System.Collections.Generic;

namespace KoiPondConstructionManagement.Model;

public partial class Sample
{
    public int SampleId { get; set; }

    public int? ConstructionTypeId { get; set; }

    public string? SampleName { get; set; }

    public string? Size { get; set; }

    public double? Price { get; set; }

    public byte[]? Image { get; set; }

    public virtual ConstructionType? ConstructionType { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
