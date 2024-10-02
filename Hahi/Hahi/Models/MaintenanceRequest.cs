using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hahi.Models
{
    public partial class MaintenanceRequest
    {
        public int MaintenanceId { get; set; }
        [Required]
        public int RequestId { get; set; }
        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
        [Required]
        public string? Status { get; set; }

        public virtual Maintenance Maintenance { get; set; } = null!;
        public virtual Request Request { get; set; } = null!;
    }
}
