

using KoiPond.DTOs;
using KoiPond.Models;

namespace KoiPond.AutoMapper
{
    public static class DesignMapper
    {
        public static DesignDtoV1 ToDesignDto(this Design design)
        {
            return new DesignDtoV1
            {
                ConstructionTypeName = design.ConstructionType?.ConstructionTypeName, 
                DesignName = design.DesignName,
                DesignSize = design.DesignSize,
                DesignPrice = design.DesignPrice,
                DesignImage = design.DesignImage
            };
        }

        public static Design ToDesignFromCreatedDto(this CreateDesignRequestDto designDto)
        {
            var design = new Design
            {
                DesignName = designDto.DesignName,
                DesignSize = designDto.DesignSize,
                DesignPrice = designDto.DesignPrice,
                DesignImage = designDto.DesignImage,

                ConstructionType = designDto.ConstructionTypes != null && designDto.ConstructionTypes.Count > 0
                    ? new ConstructionType
                    {
                        ConstructionTypeName = designDto.ConstructionTypes.First().ConstructionTypeName
                    }
                    : null
            };

            return design;
        }
    }
}
