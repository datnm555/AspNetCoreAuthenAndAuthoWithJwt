using AspNetCoreAuthenAndAuthoWithJwt.Entities;

namespace AspNetCoreAuthenAndAuthoWithJwt.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
