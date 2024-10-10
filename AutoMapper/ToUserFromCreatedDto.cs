

using KoiPond.DTOs;
using KoiPond.Models;

namespace KoiPond.AutoMapper
{
    public static class UserMappingExtensions
    {
        public static UserDto ToUserDto(this User user)
        {
            if (user.Account == null)
            {
                return null; // Or handle the null case appropriately, depending on your requirements
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
