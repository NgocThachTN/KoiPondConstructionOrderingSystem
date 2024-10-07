using Hahi.DTOs;
using Hahi.ModelsV1;

namespace Hahi.AutoMapper
{
    public static class RequestMapper
    {
        public static RequestDto ToRequestDto(this Request request)
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
    





// Mapping CreateRequestDto to Request for saving to the database
        public static Request ToRequestDesignFromCreatedDto(this CreateRequestDesignDto requestDto)
        {
            var request = new Request
            {
                RequestName = requestDto.RequestName,
                Description = requestDto.Description,

                // Assign User from requestDto
                User = new User
                {
                    Name = requestDto.User.Name,
                    PhoneNumber = requestDto.User.PhoneNumber,
                    Address = requestDto.User.Address,
                    RoleId = requestDto.User.RoleId, // Assuming RoleId is provided in UserDto

                    // Create and assign the Account to User
                    Account = new Account
                    {
                        UserName = requestDto.User.UserName,
                        Email = requestDto.User.Email,
                        Password = requestDto.User.Password
                    }
                },

                // Assign Design or Sample based on selection
                Design = requestDto.IsDesignSelected ? requestDto.Design != null ? new Design
                {
                    ConstructionType = new ConstructionType
                    {
                        ConstructionTypeName = requestDto.Design.ConstructionTypeName
                    },
                    DesignName = requestDto.Design.DesignName,
                    DesignSize = requestDto.Design.DesignSize,
                    DesignPrice = requestDto.Design.DesignPrice,
                    DesignImage = requestDto.Design.DesignImage
                } : null : null,
            };

            return request;
        }

        public static Request ToRequestSampleFromCreatedDto(this CreateRequestSampleDto requestDto)
        {
            var request = new Request
            {
                RequestName = requestDto.RequestName,
                Description = requestDto.Description,

                // Assign User from requestDto
                User = new User
                {
                    Name = requestDto.User.Name,
                    PhoneNumber = requestDto.User.PhoneNumber,
                    Address = requestDto.User.Address,
                    RoleId = requestDto.User.RoleId, // Assuming RoleId is provided in UserDto

                    // Create and assign the Account to User
                    Account = new Account
                    {
                        UserName = requestDto.User.UserName,
                        Email = requestDto.User.Email,
                        Password = requestDto.User.Password
                    }
                },

                // Assign Design or Sample based on selection
                Sample = requestDto.IsSampleSelected ? requestDto.Sample != null ? new Sample
                {
                    ConstructionType = new ConstructionType
                    {
                        ConstructionTypeName = requestDto.Sample.ConstructionTypeName
                    },
                    SampleName = requestDto.Sample.SampleName,
                    SampleSize = requestDto.Sample.SampleSize,
                    SamplePrice = requestDto.Sample.SamplePrice,
                    SampleImage = requestDto.Sample.SampleImage
                } : null : null,
            };

            return request;
        }

    }
}

