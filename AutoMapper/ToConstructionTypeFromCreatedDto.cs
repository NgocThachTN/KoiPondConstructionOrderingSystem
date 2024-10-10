

using KoiPond.DTOs;
using KoiPond.Models;

namespace KoiPond.AutoMapper
{
    public static class ConstructionTypeMapper
    {
        public static ConstructionTypeDto ToConstructionTypeDto(this ConstructionType constructionType)
        {
            return new ConstructionTypeDto
            {
                ConstructionName = constructionType.ConstructionTypeName,
                DesignName = constructionType.Designs.FirstOrDefault()?.DesignName,
                DesignSize = constructionType.Designs.FirstOrDefault()?.DesignSize,
                DesignPrice = constructionType.Designs.FirstOrDefault()?.DesignPrice,
                DesignImage = constructionType.Designs.FirstOrDefault()?.DesignImage,

                SampleName = constructionType.Samples.FirstOrDefault()?.SampleName,
                SampleSize = constructionType.Samples.FirstOrDefault()?.SampleSize,
                SamplePrice = constructionType.Samples.FirstOrDefault()?.SamplePrice,
                SampleImage = constructionType.Samples.FirstOrDefault()?.SampleImage
            };
        }

        public static ConstructionType ToConstructionTypeFromCreatedDto(this CreateConstructionTypeRequestDto constructionTypeDto)
        {
            var constructionType = new ConstructionType
            {
                ConstructionTypeName = constructionTypeDto.ConstructionName,
                Designs = constructionTypeDto.Designs.Select(d => new Design
                {
                    DesignName = d.DesignName,
                    DesignSize = d.DesignSize,
                    DesignPrice = d.DesignPrice,
                    DesignImage = d.DesignImage
                }).ToList(),
                Samples = constructionTypeDto.Samples.Select(s => new Sample
                {
                    SampleName = s.SampleName,
                    SampleSize = s.SampleSize,
                    SamplePrice = s.SamplePrice,
                    SampleImage = s.SampleImage
                }).ToList()
            };

            return constructionType;
        }
    }
}
