using KoiPondConstructionManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KoiPondConstructionManagement.Repositories
{
    public interface IAccountRepositories
    {
        public Task<IActionResult> SignUpAsync(SignUpModel model);

        public Task<string> SignInAsync(SignInModel model);

        public Task DeleteAccountAsync(int Id);

    }
}