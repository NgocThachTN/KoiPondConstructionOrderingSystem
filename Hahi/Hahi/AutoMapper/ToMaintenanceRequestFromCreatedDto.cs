using Hahi.DTOs;
using Hahi.ModelsV1;
using System.Diagnostics.Contracts;

namespace Hahi.AutoMapper
{
    public static class MaintenanceRequestAutoMapper
    {
        public static MaintenanceRequestDto ToMaintenanceDto(this MaintenanceRequest maintenanceRequest)
        {
            return new MaintenanceRequestDto
            {
                MaintenanceRequestStartDate = maintenanceRequest.MaintenanceRequestStartDate,
                MaintenanceRequestEndDate = maintenanceRequest.MaintenanceRequestEndDate,
                Status = maintenanceRequest.Status,
                Maintenance = maintenanceRequest.MaintenanceRequestNavigation != null
                    ? new List<MaintenanceDto>
                    {
                        new MaintenanceDto
                        {
                            MaintencaceName = maintenanceRequest.MaintenanceRequestNavigation.MaintencaceName,
                        }
                    }
                    : new List<MaintenanceDto>(),
                Requests = maintenanceRequest.Request != null ? new List<RequestDto>
                {
                    maintenanceRequest.Request.ToMaintenanceRequestDto() // Convert the Request entity to RequestDto
                } : new List<RequestDto>()
            };
        }

        public static RequestDto ToMaintenanceRequestDto(this Request request)
        {
            return new RequestDto
            {
                RequestName = request.RequestName,
                Description = request.Description,
                Users = request.User != null ? new List<UserDto>
                {
                    new UserDto
                    {
                        Name = request.User.Name,
                        PhoneNumber = request.User.PhoneNumber,
                        Address = request.User.Address,
                        RoleId = request.User.RoleId
                    }
                } : new List<UserDto>(),

                Designs = request.Design != null ? new List<DesignDtoV1>
                {
                    new DesignDtoV1
                    {
                        ConstructionTypeName = request.Design.ConstructionType?.ConstructionTypeName,
                        DesignName = request.Design.DesignName,
                        DesignSize = request.Design.DesignSize,
                        DesignPrice = request.Design.DesignPrice,
                        DesignImage = request.Design.DesignImage
                    }
                } : new List<DesignDtoV1>(),

                Samples = request.Sample != null ? new List<SampleDtoV1>
                {
                    new SampleDtoV1
                    {
                        ConstructionTypeName = request.Sample.ConstructionType?.ConstructionTypeName,
                        SampleName = request.Sample.SampleName,
                        SampleSize = request.Sample.SampleSize,
                        SamplePrice = request.Sample.SamplePrice,
                        SampleImage = request.Sample.SampleImage
                    }
                } : new List<SampleDtoV1>()
            };
        }

        public static MaintenanceRequest ToMaintenanceRequestDesignFromCreatedDto(this CreateMaintenanceRequestDesignDto requestDto)
        {
            var maintenanceRequest = new MaintenanceRequest
            {
                MaintenanceRequestStartDate = requestDto.MaintenanceRequestStartDate,
                MaintenanceRequestEndDate = requestDto.MaintenanceRequestEndDate,
                Status = requestDto.Status,
                Request = requestDto.Requests != null && requestDto.Requests.Count > 0
                    ? new Request
                    {
                        RequestName = requestDto.Requests.First().RequestName,
                        Description = requestDto.Requests.First().Description,
                        User = requestDto.Requests.First().Users != null && requestDto.Requests.First().Users.Count > 0
                            ? new User
                            {
                                Name = requestDto.Requests.First().Users.First().Name,
                                PhoneNumber = requestDto.Requests.First().Users.First().PhoneNumber,
                                Address = requestDto.Requests.First().Users.First().Address,
                                RoleId = requestDto.Requests.First().Users.First().RoleId
                            }
                            : null,

                        Design = requestDto.Requests.First().Designs != null && requestDto.Requests.First().Designs.Count > 0
                            ? new Design
                            {
                                DesignName = requestDto.Requests.First().Designs.First().DesignName,
                                DesignSize = requestDto.Requests.First().Designs.First().DesignSize,
                                DesignPrice = requestDto.Requests.First().Designs.First().DesignPrice,
                                DesignImage = requestDto.Requests.First().Designs.First().DesignImage
                            }
                            : null
                    }
                    : null,
        MaintenanceRequestNavigation = requestDto.Maintenance != null && requestDto.Maintenance.Count > 0
            ? new Maintenance
            {
                MaintencaceName = requestDto.Maintenance.First().MaintencaceName
            }
            : null
            };
            return maintenanceRequest;
        }


        public static MaintenanceRequest ToMaintenanceRequestSampleFromCreatedDto(this CreateMaintenanceRequestSampleDto requestDto)
        {
            var maintenanceRequest = new MaintenanceRequest
            {
                MaintenanceRequestStartDate = requestDto.MaintenanceRequestStartDate,
                MaintenanceRequestEndDate = requestDto.MaintenanceRequestEndDate,
                Status = requestDto.Status,
                Request = requestDto.Requests != null && requestDto.Requests.Count > 0 ?
                    new Request
                    {
                        RequestName = requestDto.Requests.First().RequestName,
                        Description = requestDto.Requests.First().Description,
                        User = requestDto.Requests.First().Users != null && requestDto.Requests.First().Users.Count > 0 ?
                            new User
                            {
                                Name = requestDto.Requests.First().Users.First().Name,
                                PhoneNumber = requestDto.Requests.First().Users.First().PhoneNumber,
                                Address = requestDto.Requests.First().Users.First().Address,
                                RoleId = requestDto.Requests.First().Users.First().RoleId
                            } : null,

                        Sample = requestDto.Requests.First().Samples != null && requestDto.Requests.First().Samples.Count > 0 ?
                            new Sample
                            {
                                SampleName = requestDto.Requests.First().Samples.First().SampleName,
                                SampleSize = requestDto.Requests.First().Samples.First().SampleSize,
                                SamplePrice = requestDto.Requests.First().Samples.First().SamplePrice,
                                SampleImage = requestDto.Requests.First().Samples.First().SampleImage
                            } : null
                    } : null,
        MaintenanceRequestNavigation = requestDto.Maintenance != null && requestDto.Maintenance.Count > 0
            ? new Maintenance
            {
                MaintencaceName = requestDto.Maintenance.First().MaintencaceName
            }
            : null
            };
            return maintenanceRequest;
        }
    }
}
