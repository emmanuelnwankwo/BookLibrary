using AutoMapper;
using BookLibrary.API.Models.Users;
using BookLibrary.Domain.Aggregates.UserAggregate;
using BookLibrary.Domain.DTOs;
using BookLibrary.Domain.Shared;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookLibrary.API.Services.Users
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly GeneralConfig _generalConfig;
        public AuthService(ILogger<AuthService> logger, IMapper mapper, IUserRepository userRepository, IOptions<GeneralConfig> generalConfig) 
            : base(logger, mapper)
        {
            _userRepository = userRepository;
            _generalConfig = generalConfig.Value;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _userRepository.GetAsync(x => x.Email == request.Email);
            if (user == null) throw new ArgumentException("Incorrect email or password");

            var isCorrect = user.VerifyPassword(request.Password);
            if (!isCorrect) throw new ArgumentException("Incorrect email or password");

            var token = GenerateJwtToken(user);
            var userDto = _mapper.Map<UserDto>(user);
            return new LoginResponse
            {
                User = userDto,
                Token = token,
                Expire = _generalConfig.Jwt.TokenExpireTimeInSeconds
            };
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = Encoding.ASCII.GetBytes(_generalConfig.Jwt.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { 
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.Now.AddSeconds(_generalConfig.Jwt.TokenExpireTimeInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _generalConfig.Jwt.Issuer
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
