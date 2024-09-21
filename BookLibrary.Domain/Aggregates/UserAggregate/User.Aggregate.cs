using BCrypt.Net;
using BookLibrary.Domain.DTOs;
using BookLibrary.Domain.SeedWork;

namespace BookLibrary.Domain.Aggregates.UserAggregate
{
    public partial class User : IAggregateRoot
    {
        public User AddUser(UserDto user)
        {
            var newUser = new User
            {
                Email = user.Email,
                Name = user.Name,
                Role = user.Role,
            };
            return newUser;
        }

        public void SetPassword(string password) 
        {
            var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password, HashType.SHA512);
            Password = passwordHash;
        }

        public bool VerifyPassword(string password) 
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, Password, HashType.SHA512);
        }
    }
}
