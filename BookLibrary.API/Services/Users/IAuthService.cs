using BookLibrary.API.Models.Users;

namespace BookLibrary.API.Services.Users
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest request);
    }
}
