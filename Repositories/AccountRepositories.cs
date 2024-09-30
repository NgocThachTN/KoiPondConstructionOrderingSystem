using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KoiPondConstructionManagement.Data;
using KoiPondConstructionManagement.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace KoiPondConstructionManagement.Repositories
{
    public class AccountRepositories : IAccountRepositories
    {
        Task IAccountRepositories.DeleteAccountAsync(int Id)
        {
            throw new NotImplementedException();
        }

        Task<string> IAccountRepositories.SignInAsync(SignInModel model)
        {
            throw new NotImplementedException();
        }

        Task<IActionResult> IAccountRepositories.SignUpAsync(SignUpModel model)
        {
            throw new NotImplementedException();
        }
    }
}