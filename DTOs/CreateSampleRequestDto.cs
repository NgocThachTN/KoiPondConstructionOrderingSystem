﻿namespace KoiPond.DTOs
{
    public class CreateSampleRequestDto
    {
        public List<ConstructionTypeDtoV1> ConstructionTypes { get; set; }
        public string? SampleName { get; set; }
        public string? SampleSize { get; set; }
        public double? SamplePrice { get; set; }
        public string? SampleImage { get; set; }
    }
}
