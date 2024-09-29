using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hahi.Models
{
    public partial class Sample
    {
        public Sample()
        {
            Requests = new HashSet<Request>();
        }

        public int SampleId { get; set; }
        [Required]
        public int? ConstructionTypeId { get; set; }
        [Required]
        public string? SampleName { get; set; }
        [Required]
        public string? Size { get; set; }
        [Required]
        public double? Price { get; set; }
        [Required]
        public byte[]? Image { get; set; }

        public virtual ConstructionType? ConstructionType { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
