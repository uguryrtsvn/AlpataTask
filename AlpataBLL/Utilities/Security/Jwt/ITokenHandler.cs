using AlpataEntities.Entities.Concretes; 

namespace AlpataBLL.Utilities.Security.Jwt
{
    public interface ITokenHandler
    {
        Token CreateToken(AppUser user);
    }
}
