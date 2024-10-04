using System;
using System.Collections.Generic;

namespace Hahi.ModelsV1
{
    public partial class Sample
    {
        public Sample()
        {
            Requests = new HashSet<Request>();
        }

        public int SampleId { get; set; }
        public int? ConstructionTypeId { get; set; }
        public string? SampleName { get; set; }
        public string? SampleSize { get; set; }
        public double? SamplePrice { get; set; }
        public string? SampleImage { get; set; }

        public virtual ConstructionType? ConstructionType { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
