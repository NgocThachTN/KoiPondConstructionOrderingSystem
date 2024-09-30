using System;
using System.Collections.Generic;

namespace KoiPondConstructionManagement.Model;

public partial class Request
{
    public int RequestId { get; set; }

    public int? UserId { get; set; }

    public string? RequestName { get; set; }

    public string? Description { get; set; }

    public int? SampleId { get; set; }

    public int? DesignId { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual Design? Design { get; set; }

    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();

    public virtual Sample? Sample { get; set; }

    public virtual User? User { get; set; }
}
