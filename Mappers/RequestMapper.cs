using KoiPondConstructionManagement.Dtos.Request;
using KoiPondConstructionManagement.Model;

namespace KoiPondConstructionManagement.Mappers
{
    public static class RequestMapper
    {
        public static RequestDto ToRequestDto(this Request requestModel)
        {
            return new RequestDto
            {
                RequestId = requestModel.RequestId,
                RequestName = requestModel.RequestName,
                Description = requestModel.Description
            };
        }
    }
}