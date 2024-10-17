using Hahi.DTOs;
using Hahi.ModelsV1;
using Microsoft.EntityFrameworkCore;

namespace Hahi.AutoMapper
{
    public static class RequestMapper
    {
        public static RequestDto ToRequestDto(this Request request)
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
        public static async Task<(Request request, string errorMessage)> ToRequestDesignFromCreatedDto(this CreateRequestDesignDto requestDto, KoisV1Context _context)
        {
            // Check if the RequestName already exists in the database
            var existingRequestByName = await _context.Requests
                .FirstOrDefaultAsync(r => r.RequestName == requestDto.RequestName);

            // If the RequestName does not exist in the database, return an error message
            if (existingRequestByName == null)
            {
                return (null, $"RequestName '{requestDto.RequestName}' does not match any existing request records in the database.");
            }
            // Check if the Request already exists
            var existingRequest = await _context.Requests
                .Include(r => r.User)
                .Include(r => r.Design)
                .FirstOrDefaultAsync(r => r.RequestId == requestDto.RequestId);

            // If the request exists, use it; otherwise, create a new one
            var request = existingRequest ?? new Request
            {
                RequestName = requestDto.RequestName,
                Description = requestDto.Description,
            };

            // Check if the User already exists
            var existingUser = await _context.Users
                .Include(u => u.Account)
                .FirstOrDefaultAsync(u => u.Name == requestDto.User.Name &&
                                          u.PhoneNumber == requestDto.User.PhoneNumber &&
                                          u.Account.Email == requestDto.User.Email &&
                                          u.Address == requestDto.User.Address);

            // If the user does not exist in the database and the request requires a valid user, return an error message
            if (existingUser == null)
            {
                return (null, "User data provided does not match any existing user records in the database.");
            }

            // If the user exists, use it; otherwise, create a new one
            request.User = existingUser ?? new User
            {
                Name = requestDto.User.Name,
                PhoneNumber = requestDto.User.PhoneNumber,
                Address = requestDto.User.Address,
                Account = new Account
                {
                    UserName = requestDto.User.UserName,
                    Email = requestDto.User.Email,
                    Password = requestDto.User.Password
                },
                RoleId = requestDto.User.RoleId
            };

            // Check if the Design already exists
            if (requestDto.IsDesignSelected && requestDto.Design != null)
            {
                var existingDesign = await _context.Designs
                    .FirstOrDefaultAsync(d => d.DesignName == requestDto.Design.DesignName &&
                                              d.DesignSize == requestDto.Design.DesignSize);

                // If the design does not exist in the database and the request requires a valid design, return an error message
                if (existingDesign == null)
                {
                    return (null, "Design data provided does not match any existing design records in the database.");
                }

                // If the design exists, use it; otherwise, create a new one
                request.Design = existingDesign ?? new Design
                {
                    DesignName = requestDto.Design.DesignName,
                    DesignSize = requestDto.Design.DesignSize,
                    DesignPrice = requestDto.Design.DesignPrice,
                    DesignImage = requestDto.Design.DesignImage,
                };
            }

            return (request, null); // Return the request object if all validations pass
        }


        // Mapping CreateRequestSampleDto to Request for saving to the database with error handling
        public static async Task<(Request request, string errorMessage)> ToRequestSampleFromCreatedDtoAsync(this CreateRequestSampleDto requestDto, KoisV1Context context)
        {
            // Check if the RequestName already exists in the database
            var existingRequestByName = await context.Requests
                .FirstOrDefaultAsync(r => r.RequestName == requestDto.RequestName);

            // If the RequestName does not exist in the database, return an error message
            if (existingRequestByName == null)
            {
                return (null, $"RequestName '{requestDto.RequestName}' does not match any existing request records in the database.");
            }

            // Check if the User already exists
            var existingUser = await context.Users
                .Include(u => u.Account)
                .FirstOrDefaultAsync(u => u.Name == requestDto.User.Name &&
                                          u.PhoneNumber == requestDto.User.PhoneNumber &&
                                          u.Account.Email == requestDto.User.Email &&
                                          u.Address == requestDto.User.Address);

            if (existingUser == null)
            {
                return (null, "User data provided does not match any existing user records in the database.");
            }

            // Check if the Sample already exists
            var existingSample = await context.Samples
                .FirstOrDefaultAsync(s => s.SampleName == requestDto.Sample.SampleName &&
                                          s.SampleSize == requestDto.Sample.SampleSize);

            if (requestDto.IsSampleSelected && existingSample == null)
            {
                return (null, "Sample data provided does not match any existing sample records in the database.");
            }

            // Create a new Request object with the existing or new data
            var request = new Request
            {
                RequestName = requestDto.RequestName,
                Description = requestDto.Description,
                User = existingUser ?? new User
                {
                    Name = requestDto.User.Name,
                    PhoneNumber = requestDto.User.PhoneNumber,
                    Address = requestDto.User.Address,
                    Account = new Account
                    {
                        UserName = requestDto.User.UserName,
                        Email = requestDto.User.Email,
                        Password = requestDto.User.Password
                    },
                    RoleId = requestDto.User.RoleId,
                },
                Sample = requestDto.IsSampleSelected ? existingSample ?? new Sample
                {
                    SampleName = requestDto.Sample.SampleName,
                    SampleSize = requestDto.Sample.SampleSize,
                    SamplePrice = requestDto.Sample.SamplePrice,
                    SampleImage = requestDto.Sample.SampleImage
                } : null,
            };

            return (request, null); // Return the request object if all validations pass
        }
    }
}

