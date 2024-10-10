using System;
using System.Collections.Generic;

namespace KoiPond.Models;

public partial class Contract
{
    public int ContractId { get; set; }

    public int? RequestId { get; set; }

    public string? ContractName { get; set; }

    public DateTime? ContractStartDate { get; set; }

    public DateTime? ContractEndDate { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }

    public virtual Request? Request { get; set; }
}
