using BookLibrary.Domain.DTOs;

namespace BookLibrary.API.Models.Users
{
    public class LoginResponse
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
        public int Expire { get; set; }
        public string RefreshToken { get; set; }
    }
}
