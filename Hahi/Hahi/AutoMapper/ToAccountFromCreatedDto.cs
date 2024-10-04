using Hahi.DTOs;
using Hahi.ModelsV1;

namespace Hahi.AutoMapper
{
    public static class AccountMapper
    {
        public static AccountDto ToAccountDto(this Account account)
        {
            // Check if Account is null
            if (account.User == null)
            {
                throw new InvalidOperationException("Related User not found."); // Or handle it appropriately
            }
            return new AccountDto
            {
                UserName = account.UserName,
                Email = account.Email,
                Password = account.Password,
                Name = account.User.Name,
                PhoneNumber = account.User.PhoneNumber,
                Address = account.User.Address,
                RoleId = account.User.RoleId
            };
        }
        public static Account ToAccountFromCreatedDto(this CreateAccountRequestDto accountDto)
        {
            return new Account
            {
                UserName = accountDto.UserName,
                Email = accountDto.Email,
                Password = accountDto.Password,
                User = new User
                {
                    Name = accountDto.Name,
                    Address = accountDto.Address,
                    PhoneNumber = accountDto.PhoneNumber,
                    RoleId = accountDto.RoleId
                }
            };
        }
    }
}
