using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hahi.Models
{
    public partial class Request
    {
        public Request()
        {
            MaintenanceRequests = new HashSet<MaintenanceRequest>();
        }

        public int RequestId { get; set; }
        [Required]
        public int? UserId { get; set; }
        [Required]
        public string? RequestName { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public int? SampleId { get; set; }
        [Required]
        public int? DesignId { get; set; }

        public virtual Design? Design { get; set; }
        public virtual Sample? Sample { get; set; }
        public virtual User? User { get; set; }
        public virtual Contract? Contract { get; set; }
        public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; }
    }
}
