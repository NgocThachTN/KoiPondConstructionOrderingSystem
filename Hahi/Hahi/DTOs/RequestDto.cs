using System.ComponentModel.DataAnnotations;

namespace Hahi.DTOs
{
    public class RequestDto
    {
        public List<UserDto> Users { get; set; }
        public List<DesignDtoV1> Designs { get; set; }

        public List<SampleDtoV1> Samples { get; set; }
        public int RequestId {  get; set; }
        public string? RequestName { get; set; }
        public string? Description { get; set; }
    }
}
