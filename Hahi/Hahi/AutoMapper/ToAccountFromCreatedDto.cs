using Hahi.DTOs;
using Hahi.Models;

namespace Hahi.AutoMapper
{
    public static class AccountMapper
    {
        public static AccountDto ToAccountDto(this Account account)
        {
            return new AccountDto
            {
                UserName = account.UserName,
                Email = account.Email,
                Password = account.Password
            };
        }
        public static Account ToAccountFromCreatedDto(this CreateAccountRequestDto accountDto)
        {
            return new Account
            {
                UserName = accountDto.UserName,
                Email = accountDto.Email,
                Password = accountDto.Password
            };
        }
    }
}
