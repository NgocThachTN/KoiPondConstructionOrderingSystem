namespace Hahi.DTOs
{
    public class MaintenanceRequestDto
    {
        public List<RequestDto> Requests { get; set; }
        public List<MaintenanceDto> Maintenance { get; set; }
        public DateTime? MaintenanceRequestStartDate { get; set; }
        public DateTime? MaintenanceRequestEndDate { get; set; }
        public string? Status { get; set; }
    }
}
