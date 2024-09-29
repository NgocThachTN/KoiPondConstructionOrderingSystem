using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hahi.Models
{
    public partial class Maintenance
    {
        public Maintenance()
        {
            MaintenanceRequests = new HashSet<MaintenanceRequest>();
        }

        public int MaintenanceId { get; set; }
        [Required]
        public string? MaintencaceName { get; set; }

        public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; }
    }
}
