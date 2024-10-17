namespace Hahi.DTOs
{
    public class CreateMaintenanceRequestDesignDto
    {
        public List<MaintenanceRequestDesignDto> Requests { get; set; }
        public List<MaintenanceDto> Maintenance { get; set; }
        public int MaintenanceRequestId { get; set; }
        public DateTime? MaintenanceRequestStartDate { get; set; }
        public DateTime? MaintenanceRequestEndDate { get; set; }
        public string? Status { get; set; }
    }

    public class MaintenanceRequestDesignDto
    {
        public List<UserDto> Users { get; set; }
        public List<DesignDtoV1> Designs { get; set; }
        public int RequestId { get; set; }
        public string? RequestName { get; set; }
        public string? Description { get; set; }
    }
}
