﻿
using KoiPond.DTOs;
using KoiPond.Models;

namespace KoiPond.AutoMapper
{
    public static class SampleMapper
    {
        public static SampleDtoV1 ToSampleDto(this Sample sample)
        {
            return new SampleDtoV1
            {
                SampleName = sample.SampleName,
                SampleSize = sample.SampleSize,
                SamplePrice = sample.SamplePrice,
                SampleImage = sample.SampleImage
            };
        }

        public static Sample ToSampleFromCreatedDto(this CreateSampleRequestDto sampleDto)
        {
            var sample = new Sample
            {
                SampleName = sampleDto.SampleName,
                SampleSize = sampleDto.SampleSize,
                SamplePrice = sampleDto.SamplePrice,
                SampleImage = sampleDto.SampleImage,

                ConstructionType = sampleDto.ConstructionTypes != null && sampleDto.ConstructionTypes.Count > 0
                    ? new ConstructionType
                    {
                        ConstructionTypeName = sampleDto.ConstructionTypes.First().ConstructionTypeName
                    }
                    : null
            };

            return sample;
        }
    }
}
