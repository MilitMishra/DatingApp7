

using API.Entities;

namespace API.IServices
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}