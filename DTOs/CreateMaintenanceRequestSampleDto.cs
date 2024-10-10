namespace KoiPond.DTOs
{
    public class CreateMaintenanceRequestSampleDto
    {
        public List<MaintenanceRequestSampleDto> Requests { get; set; }
        public List<MaintenanceDto> Maintenance { get; set; }
        public DateTime? MaintenanceRequestStartDate { get; set; }
        public DateTime? MaintenanceRequestEndDate { get; set; }
        public string? Status { get; set; }
    }

    public class MaintenanceRequestSampleDto
    {
        public List<UserDto> Users { get; set; }
        public List<SampleDtoV1> Samples { get; set; }
        public string? RequestName { get; set; }
        public string? Description { get; set; }
    }
}
