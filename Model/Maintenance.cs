using System;
using System.Collections.Generic;

namespace KoiPondConstructionManagement.Model;

public partial class Maintenance
{
    public int MaintenanceId { get; set; }

    public string? MaintencaceName { get; set; }

    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();
}
