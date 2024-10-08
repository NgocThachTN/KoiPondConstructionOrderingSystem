using Hahi.DTOs;
using Hahi.ModelsV1;

namespace Hahi.AutoMapper
{
    public static class AccountMapper
    {
        public static AccountDto ToAccountDto(this Account account)
        {
            if (account.User == null)
            {
                return null; // Or return a default AccountDto if needed
            }

            return new AccountDto
            {
                UserName = account.User.Account?.UserName,
                Email = account.User.Account?.Email,
                Password = account.User.Account?.Password,
                Name = account.User.Name,
                PhoneNumber = account.User.PhoneNumber,
                Address = account.User.Address,
                RoleId = account.User.RoleId,
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
