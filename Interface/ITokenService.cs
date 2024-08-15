using API.Dto.Account;
using API.Models;

namespace API.Interface
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
        Task<bool> EmailExists(string email);
        Task<bool> UserNameExists(string userName);
    }
}
