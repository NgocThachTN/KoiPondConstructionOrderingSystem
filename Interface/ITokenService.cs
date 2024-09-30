

using KoiPondConstructionManagement.Model;

namespace KoiPondConstructionManagement.Interface
{
    public interface ITokenService
    {
        string CreateTokenUser(Account account);
    }
}