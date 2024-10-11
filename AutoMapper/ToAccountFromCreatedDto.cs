
using KoiPond.DTOs;
using KoiPond.Models;

namespace KoiPond.AutoMapper
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
                AccountId = account.AccountId,
                UserName = account.UserName,
                Email = account.Email,
                Password = account.Password,
                Name = account.User?.Name ?? string.Empty,
                PhoneNumber = account.User?.PhoneNumber ?? string.Empty,
                Address = account.User?.Address ?? string.Empty,
                RoleId = account.User?.RoleId ?? 1 // Default RoleId to 1 if null
            };
        }

        public static Account ToAccountFromCreatedDto(this CreateAccountRequestDto accountDto)
        {
            return new Account
            {
                AccountId = accountDto.AccountId,
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