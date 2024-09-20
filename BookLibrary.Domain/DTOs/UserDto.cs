using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.Domain.DTOs
{
    public class UserDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
