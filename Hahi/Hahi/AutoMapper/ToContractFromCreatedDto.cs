using Hahi.DTOs;
using Hahi.ModelsV1;
using System.Collections.Generic; // For List<T>
using System.Linq;

namespace Hahi.AutoMapper
{
    public static class ContractMapper
    {
        public static ContractDto ToContractDto(this Contract contract)
        {
            return new ContractDto
            {
                ContractId = contract.ContractId,
                ContractName = contract.ContractName,
                ContractStartDate = contract.ContractStartDate,
                ContractEndDate = contract.ContractEndDate,
                Status = contract.Status,
                Description = contract.Description,
                Requests = contract.Request != null ? new List<RequestDto>
                {
                    contract.Request.ToContractRequestDto() // Convert the Request entity to RequestDto
                } : new List<RequestDto>()
            };
        }

        public static RequestDto ToContractRequestDto(this Request request)
        {
            return new RequestDto
            {
                RequestId = request.RequestId,
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
                        UserName = request.User.Account?.UserName,
                        Email = request.User.Account?.Email,
                        Password = request.User.Account?.Password,
                        RoleId = request.User.RoleId
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

        public static Request ToRequestFromContractRequestDto(this RequestDto requestDto)
        {
            var request = new Request
            {
                RequestName = requestDto.RequestName,
                Description = requestDto.Description,
                User = requestDto.Users != null && requestDto.Users.Count > 0 ?
                    new User
                    {
                        Name = requestDto.Users.First().Name,
                        PhoneNumber = requestDto.Users.First().PhoneNumber,
                        Address = requestDto.Users.First().Address,
                        Account = new Account
                        {
                            UserName = requestDto.Users.First().UserName,
                            Email = requestDto.Users.First().Email,
                            Password = requestDto.Users.First().Password
                        },
                        RoleId = requestDto.Users.First().RoleId
                    } : null,
                Design = requestDto.Designs != null && requestDto.Designs.Count > 0 ?
                    new Design
                    {
                        DesignName = requestDto.Designs.First().DesignName,
                        DesignSize = requestDto.Designs.First().DesignSize,
                        DesignPrice = requestDto.Designs.First().DesignPrice,
                        DesignImage = requestDto.Designs.First().DesignImage,
                        ConstructionType = requestDto.Designs.First().ConstructionTypeName != null ?
                            new ConstructionType
                            {
                                ConstructionTypeName = requestDto.Designs.First().ConstructionTypeName
                            } : null
                    } : null,
                Sample = requestDto.Samples != null && requestDto.Samples.Count > 0 ?
                    new Sample
                    {
                        SampleName = requestDto.Samples.First().SampleName,
                        SampleSize = requestDto.Samples.First().SampleSize,
                        SamplePrice = requestDto.Samples.First().SamplePrice,
                        SampleImage = requestDto.Samples.First().SampleImage,
                        ConstructionType = requestDto.Samples.First().ConstructionTypeName != null ?
                            new ConstructionType
                            {
                                ConstructionTypeName = requestDto.Samples.First().ConstructionTypeName
                            } : null
                    } : null
            };

            return request;
        }


        public static Contract ToContractDesignFromCreatedDto(this CreateContractDesignDto requestDto, Request existingRequest, User existingUser, Design existingDesign)
        {
            var contract = new Contract
            {
                ContractId = requestDto.ContractId,
                ContractName = requestDto.ContractName,
                ContractStartDate = requestDto.ContractStartDate,
                ContractEndDate = requestDto.ContractEndDate,
                Description = requestDto.Description,
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
                }
            };

            return contract;
        }



        public static Contract ToContractSampleFromCreatedDto(this CreateContractSampleDto requestDto, Request existingRequest, User existingUser, Sample existingSample)
        {
            var contract = new Contract
            {
                ContractId = requestDto.ContractId,
                ContractName = requestDto.ContractName,
                ContractStartDate = requestDto.ContractStartDate,
                ContractEndDate = requestDto.ContractEndDate,
                Description = requestDto.Description,
                Status = requestDto.Status,
                Request = existingRequest ?? new Request
                {
                    RequestName = requestDto.Requests.First().RequestName,
                    Description = requestDto.Requests.First().Description,
                    User = existingUser ?? new User
                    {
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
                    Sample = existingSample ?? new Sample
                    {
                        SampleName = requestDto.Requests.First().Samples.First().SampleName,
                        SampleSize = requestDto.Requests.First().Samples.First().SampleSize,
                        SamplePrice = requestDto.Requests.First().Samples.First().SamplePrice,
                        SampleImage = requestDto.Requests.First().Samples.First().SampleImage
                    }
                }
            };

            return contract;
        }
    }
}
