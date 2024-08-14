using API.Models;

namespace API.Interface
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
