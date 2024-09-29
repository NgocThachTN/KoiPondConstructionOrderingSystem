using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hahi.Models
{
    public partial class ConstructionType
    {
        public ConstructionType()
        {
            Designs = new HashSet<Design>();
            Samples = new HashSet<Sample>();
        }
        [Required]
        public int ConstructionTypeId { get; set; }
        [Required]
        public string? ConstructionName { get; set; }
        [Required]
        public virtual ICollection<Design> Designs { get; set; }
        [Required]
        public virtual ICollection<Sample> Samples { get; set; }
    }
}
