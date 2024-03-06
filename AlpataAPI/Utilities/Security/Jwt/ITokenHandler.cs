using AlpataEntities.Entities.Concretes; 

namespace AlpataAPI.Utilities.Security.Jwt
{
    public interface ITokenHandler
    {
        Token CreateToken(AppUser user);
    }
}
