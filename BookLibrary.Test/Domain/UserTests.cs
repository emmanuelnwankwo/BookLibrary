using BCrypt.Net;
using BookLibrary.Domain.Aggregates.UserAggregate;
using BookLibrary.Domain.DTOs;
using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.Test.Domain
{
    public class UserTests
    {
        [Test]
        public void AddUser_ShouldCreateNewUserWithCorrectProperties()
        {
            // Arrange
            var userDto = new UserDto
            {
                Email = "test@example.com",
                Name = "Test User",
                Role = UserRole.User
            };

            // Act
            var user = new User().AddUser(userDto);

            // Assert
            Assert.That(user.Email, Is.EqualTo(userDto.Email));
            Assert.That(user.Name, Is.EqualTo(userDto.Name));
            Assert.That(user.Role, Is.EqualTo(userDto.Role));
        }

        [Test]
        public void SetPassword_ShouldHashPasswordCorrectly()
        {
            // Arrange
            var user = new User();
            var password = "StrongPassword123!";

            // Act
            user.SetPassword(password);

            // Assert
            Assert.True(BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password, HashType.SHA512));
        }

        [Test]
        public void VerifyPassword_ShouldReturnTrueForCorrectPassword()
        {
            // Arrange
            var user = new User();
            var password = "CorrectPassword";
            user.SetPassword(password);

            // Act
            var isPasswordCorrect = user.VerifyPassword(password);

            // Assert
            Assert.True(isPasswordCorrect);
        }

        [Test]
        public void VerifyPassword_ShouldReturnFalseForIncorrectPassword()
        {
            // Arrange
            var user = new User();
            var password = "CorrectPassword";
            user.SetPassword(password);
            var wrongPassword = "WrongPassword";

            // Act
            var isPasswordCorrect = user.VerifyPassword(wrongPassword);

            // Assert
            Assert.False(isPasswordCorrect);
        }
    }
}
