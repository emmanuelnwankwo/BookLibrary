using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.Domain.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; set; }
    }
}
