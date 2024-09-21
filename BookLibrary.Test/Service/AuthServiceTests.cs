using AutoMapper;
using BookLibrary.API.Models.Users;
using BookLibrary.API.Services.Users;
using BookLibrary.Domain.Aggregates.UserAggregate;
using BookLibrary.Domain.DTOs;
using BookLibrary.Domain.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.Test.Service
{
    public class AuthServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<User> _userMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IOptions<GeneralConfig>> _generalConfigMock;
        private Mock<ILogger<AuthService>> _loggerMock;
        private AuthService _authService;
        private GeneralConfig _generalConfig;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<AuthService>>();
            _userMock = new Mock<User>();

            _generalConfig = new GeneralConfig
            {
                Jwt = new JWT
                {
                    Key = "SuperSecretSuperSecretSuperSecretSuperSecret",
                    TokenExpireTimeInSeconds = 3600,
                    Issuer = "test_issuer"
                }
            };
            _generalConfigMock = new Mock<IOptions<GeneralConfig>>();
            _generalConfigMock.Setup(x => x.Value).Returns(_generalConfig);

            _authService = new AuthService(_loggerMock.Object, _mapperMock.Object, _userRepositoryMock.Object, _generalConfigMock.Object);
        }

        //[Test]
        public async Task Login_ShouldReturnTokenAndUser_WhenCredentialsAreCorrect()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var userDto = new UserDto
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Role = UserRole.Admin
            };
            var user = new User().AddUser(userDto);

            _userRepositoryMock
                .Setup(x => x.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

            _mapperMock
                .Setup(x => x.Map<UserDto>(It.IsAny<User>()))
                .Returns(new UserDto { Email = request.Email });

            //_userMock
            //    .Setup(x => x.VerifyPassword(It.IsAny<string>()))
            //    .Returns(true);

            // Act
            var response = await _authService.Login(request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Token);
            Assert.AreEqual(response.User.Email, request.Email);
            Assert.AreEqual(response.Expire, _generalConfig.Jwt.TokenExpireTimeInSeconds);
        }

        [Test]
        public void Login_ShouldThrowException_WhenEmailIsIncorrect()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "wrong@example.com",
                Password = "password123"
            };

            _userRepositoryMock
                .Setup(x => x.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((User)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _authService.Login(request));
            Assert.That(ex.ParamName, Is.EqualTo("Incorrect email or password"));
        }

        //[Test]
        public void Login_ShouldThrowException_WhenPasswordIsIncorrect()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            var userDto = new UserDto
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Role = UserRole.Admin
            };
            var user = new User().AddUser(userDto);

            _userRepositoryMock
                .Setup(x => x.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

            //_userRepositoryMock
            //    .Setup(x => x.VerifyPassword(It.IsAny<string>()))
            //    .Returns(false);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _authService.Login(request));
            Assert.That(ex.Message, Is.EqualTo("Incorrect email or password"));
        }

        [Test]
        public void GenerateJwtToken_ShouldCreateValidToken()
        {
            // Arrange
            var userDto = new UserDto
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Role = UserRole.Admin
            };
            var user = new User().AddUser(userDto);

            // Act
            var token = CallGenerateJwtToken(user);

            // Assert
            Assert.IsNotNull(token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = Encoding.ASCII.GetBytes(_generalConfig.Jwt.Key);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _generalConfig.Jwt.Issuer,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(securityKey)
            };

            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            Assert.IsNotNull(validatedToken);
        }

        // Helper method to access the private GenerateJwtToken method
        private string CallGenerateJwtToken(User user)
        {
            var method = typeof(AuthService).GetMethod("GenerateJwtToken", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (string)method.Invoke(_authService, new object[] { user });
        }
    }
}
