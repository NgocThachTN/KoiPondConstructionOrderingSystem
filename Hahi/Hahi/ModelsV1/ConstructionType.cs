using System;
using System.Collections.Generic;

namespace Hahi.ModelsV1
{
    public partial class ConstructionType
    {
        public ConstructionType()
        {
            Designs = new HashSet<Design>();
            Samples = new HashSet<Sample>();
        }

        public int ConstructionTypeId { get; set; }
        public string? ConstructionTypeName { get; set; }

        public virtual ICollection<Design> Designs { get; set; }
        public virtual ICollection<Sample> Samples { get; set; }
    }
}
