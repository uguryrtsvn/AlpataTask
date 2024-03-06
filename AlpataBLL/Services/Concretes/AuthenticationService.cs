
using AlpataBLL.BaseResult.Abstracts;
using AlpataBLL.BaseResult.Concretes;
using AlpataBLL.Constants;
using AlpataBLL.Services.Abstracts;
using AlpataBLL.Services.EmailService;
using AlpataBLL.Utilities.Hashing;
using AlpataBLL.Utilities.Security.Jwt;
using AlpataDAL.IRepositories;
using AlpataEntities.Dtos.AuthDtos;
using AlpataEntities.Entities.Concretes;
using AutoMapper;
using System.Globalization;

namespace AlpataBLL.Services.Concretes
{
    public class AuthenticationService : IAuthenticationService
    { 
        private readonly ITokenHandler _tokenHandler;
        private readonly IAppUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AuthenticationService(ITokenHandler tokenHandler, IAppUserRepository userService, IMapper mapper, IEmailService emailService)
        {
            _tokenHandler = tokenHandler;
            _userRepository = userService;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<IDataResult<Token>> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetAsync(x => x.Email == loginDto.Email);
            if (user is null)
                return new ErrorDataResult<Token>(Messages.LoginFailed);

            var passwordVerified = HashingHelper.VerifyPasswordHash(loginDto.Password ?? string.Empty, user.PasswordHash, user.PasswordSalt);

            if (passwordVerified)
            {
                var token = _tokenHandler.CreateToken(user); 
                return new SuccessDataResult<Token>(token, Messages.LoginSuccess);
            }

            return new ErrorDataResult<Token>(Messages.LoginFailed);
        }

        public async Task<IDataResult<bool>> Register(RegisterDto dto)
        {
            var normalizedEmail = dto.Email.ToUpper(new CultureInfo("en-US"));
            var isUserExist = await _userRepository.AnyAsync(x => x.NormalizedEmail == normalizedEmail);
            if (isUserExist) return new ErrorDataResult<bool>(false, string.Format(Messages.EmailAlreadyExist, dto.Email));

            HashingHelper.CreatePassword(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var entity = _mapper?.Map<AppUser>(dto);
            entity.Name = FormatName(dto.Name); 
            entity.PasswordHash = passwordHash;
            entity.PasswordSalt = passwordSalt;
            entity.EmailConfirmed = false;
            entity.NormalizedEmail = normalizedEmail;

            var addResult = await _userRepository.CreateAsync(entity);

            if (addResult)
            {
                await _emailService.SendEmail(new EmailModel()
                {
                    Content = $"{FormatName($"{dto.Name} {dto.Surname}")} Tebrikler kaydınız tamamlandı.",
                    Receiver = dto.Email,
                    Subject = "Alpata Kaydınız Oluşturuldu."
                });
                return new SuccessDataResult<bool>(true,Messages.RegisterSuccess);
            }

            return new ErrorDataResult<bool>(false,Messages.RegisterFailed);
        }
        private string FormatName(string fullName)
        {
            string[] nameParts = fullName.Split(' ');

            for (int i = 0; i < nameParts.Length; i++)
            {
                if (!string.IsNullOrEmpty(nameParts[i]))
                {
                    nameParts[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nameParts[i].ToLower());
                }
            }
            string formattedName = string.Join(" ", nameParts);

            return formattedName;
        }
    }
}


