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
                MaintenanceRequestId = maintenanceRequest.MaintenanceRequestId,
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
                        UserId = request.User.UserId,
                        Name = request.User.Name,
                        PhoneNumber = request.User.PhoneNumber,
                        Address = request.User.Address,
                        RoleId = request.User.RoleId,
                        UserName = request.User.Account?.UserName,
                        Email = request.User.Account?.Email,
                        Password = request.User.Account?.Password
                    }
                } : new List<UserDto>(),

                Designs = request.Design != null ? new List<DesignDtoV1>
                {
                    new DesignDtoV1
                    {
                        ConstructionTypeName = request.Design.ConstructionType?.ConstructionTypeName,
                        DesignId = request.Design.DesignId,
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
                        SampleId = request.Sample.SampleId,
                        SampleName = request.Sample.SampleName,
                        SampleSize = request.Sample.SampleSize,
                        SamplePrice = request.Sample.SamplePrice,
                        SampleImage = request.Sample.SampleImage
                    }
                } : new List<SampleDtoV1>()
            };
        }

        public static MaintenanceRequest ToMaintenanceRequestDesignFromCreatedDto(this CreateMaintenanceRequestDesignDto requestDto, Request existingRequest, User existingUser, Design existingDesign, Maintenance existingMaintenance)
        {
            var maintenanceRequest = new MaintenanceRequest
            {
                MaintenanceRequestId = requestDto.MaintenanceRequestId,
                MaintenanceRequestStartDate = requestDto.MaintenanceRequestStartDate,
                MaintenanceRequestEndDate = requestDto.MaintenanceRequestEndDate,
                Status = requestDto.Status,
                Request = existingRequest ?? new Request
                {
                    RequestId = requestDto.Requests.First().RequestId,
                    RequestName = requestDto.Requests.First().RequestName,
                    Description = requestDto.Requests.First().Description,
                    User = existingUser ?? new User
                    {
                        UserId = requestDto.Requests.First().Users.First().UserId,
                        Name = requestDto.Requests.First().Users.First().Name,
                        PhoneNumber = requestDto.Requests.First().Users.First().PhoneNumber,
                        Address = requestDto.Requests.First().Users.First().Address,
                        Account = new Account
                        {
                            UserName = requestDto.Requests.First().Users.First().UserName,
                            Email = requestDto.Requests.First().Users.First().Email,
                            Password = requestDto.Requests.First().Users.First().Password
                        },
                        RoleId = requestDto.Requests.First().Users.First().RoleId
                    },
                    Design = existingDesign ?? new Design
                    {
                        DesignId = requestDto.Requests.First().Designs.First().DesignId,
                        DesignName = requestDto.Requests.First().Designs.First().DesignName,
                        DesignSize = requestDto.Requests.First().Designs.First().DesignSize,
                        DesignPrice = requestDto.Requests.First().Designs.First().DesignPrice,
                        DesignImage = requestDto.Requests.First().Designs.First().DesignImage
                    }
                },
                MaintenanceRequestNavigation = existingMaintenance ?? new Maintenance
                {
                    MaintencaceName = requestDto.Maintenance.First().MaintencaceName
                }
            };

            return maintenanceRequest;
        }



        public static MaintenanceRequest ToMaintenanceRequestSampleFromCreatedDto(this CreateMaintenanceRequestSampleDto requestDto, Request existingRequest, User existingUser, Sample existingSample, Maintenance existingMaintenance)
        {
            var maintenanceRequest = new MaintenanceRequest
            {
                MaintenanceRequestId = requestDto.MaintenanceRequestId,
                MaintenanceRequestStartDate = requestDto.MaintenanceRequestStartDate,
                MaintenanceRequestEndDate = requestDto.MaintenanceRequestEndDate,
                Status = requestDto.Status,
                Request = requestDto.Requests != null && requestDto.Requests.Count > 0 ?
                    new Request
                    {
                        RequestId = requestDto.Requests.First().RequestId,
                        RequestName = requestDto.Requests.First().RequestName,
                        Description = requestDto.Requests.First().Description,
                        User = requestDto.Requests.First().Users != null && requestDto.Requests.First().Users.Count > 0 ?
                            new User
                            {
                                UserId = requestDto.Requests.First().Users.First().UserId,
                                Name = requestDto.Requests.First().Users.First().Name,
                                PhoneNumber = requestDto.Requests.First().Users.First().PhoneNumber,
                                Address = requestDto.Requests.First().Users.First().Address,
                                Account = new Account
                                {
                                    UserName = requestDto.Requests.First().Users.First().UserName,
                                    Email = requestDto.Requests.First().Users.First().Email,
                                    Password = requestDto.Requests.First().Users.First().Password
                                },
                                RoleId = requestDto.Requests.First().Users.First().RoleId
                            } : null,

                        Sample = requestDto.Requests.First().Samples != null && requestDto.Requests.First().Samples.Count > 0 ?
                            new Sample
                            {
                                SampleId = requestDto.Requests.First().Samples.First().SampleId,
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
