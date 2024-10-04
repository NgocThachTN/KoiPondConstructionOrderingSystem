namespace Hahi.DTOs
{
    public class UpdateDesignRequestDto
    {
        public List<ConstructionTypeDtoV1> ConstructionTypes { get; set; }
        public string? DesignName { get; set; }
        public string? DesignSize { get; set; }
        public double? DesignPrice { get; set; }
        public string? DesignImage { get; set; }
    }
}
