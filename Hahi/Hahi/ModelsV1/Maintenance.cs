using System;
using System.Collections.Generic;

namespace Hahi.ModelsV1
{
    public partial class Maintenance
    {
        public Maintenance()
        {
            MaintenanceRequests = new HashSet<MaintenanceRequest>();
        }

        public int MaintenanceId { get; set; }
        public string? MaintencaceName { get; set; }

        public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; }
    }
}
