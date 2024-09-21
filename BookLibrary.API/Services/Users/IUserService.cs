using BookLibrary.API.Models.Users;
using BookLibrary.Domain.Aggregates.UserAggregate;
using BookLibrary.Domain.DTOs;

namespace BookLibrary.API.Services.Users
{
    public interface IUserService
    {
        Task<UserDto> CreateUser(AddUserRequest request);
        Task<User> GetUserByEmail(string email);
    }
}
