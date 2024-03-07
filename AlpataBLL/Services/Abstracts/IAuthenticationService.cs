
using AlpataBLL.BaseResult.Abstracts;
using AlpataBLL.Utilities.Security.Jwt;
using AlpataEntities.Dtos.AuthDtos;

namespace AlpataBLL.Services.Abstracts
{
    public interface IAuthenticationService
    {
        Task<IDataResult<Token>> Login(LoginDto loginDto);
        Task<IDataResult<RegisterDto>> Register(RegisterDto dto); 
    }
}
