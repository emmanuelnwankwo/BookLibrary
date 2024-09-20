using BookLibrary.Domain.DTOs;
using BookLibrary.Domain.SeedWork;

namespace BookLibrary.Domain.Aggregates.UserAggregate
{
    public partial class User : IAggregateRoot
    {
        public User Add(UserDto user)
        {
            var newUser = new User
            {
                Email = user.Email,
                Name = user.Name,
                Role = user.Role,
            };
            SetPassword(user.Password);
            return newUser;
        }

        public void SetPassword(string password) 
        { 
            // TODO: Hash password logic here
            Password = password;
        }
    }
}
