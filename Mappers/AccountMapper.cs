using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiPondConstructionManagement.Dtos.Account;
using KoiPondConstructionManagement.Model;

namespace KoiPondConstructionManagement.Mappers
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