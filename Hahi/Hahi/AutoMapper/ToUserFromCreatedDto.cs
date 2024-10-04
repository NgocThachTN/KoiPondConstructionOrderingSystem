using Hahi.DTOs;
using Hahi.ModelsV1;
using System.Security.Principal;
using System.Linq;

namespace Hahi.AutoMapper
{
    public static class UserMappingExtensions
    {
        public static UserDto ToUserDto(this User user)
        {
            // Check if Account is null
            if (user.Account == null)
            {
                throw new InvalidOperationException("Related Account not found."); // Or handle it appropriately
            }

            return new UserDto
            {
                UserName = user.Account.UserName,
                Email = user.Account.Email,
                Password = user.Account.Password,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                RoleId = user.RoleId
            };
        }


        public static User ToUserFromCreatedDto(this CreateUserRequestDto userDto)
        {
            return new User
            {
                Name = userDto.Name,
                PhoneNumber = userDto.PhoneNumber,
                Address = userDto.Address,
                RoleId = userDto.RoleId,
                Account = new Account
                {
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    Password = userDto.Password
                }
            };
        }
    }
}
