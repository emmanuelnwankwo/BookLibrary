using BookLibrary.Domain.SeedWork;
using System.ComponentModel.DataAnnotations.Schema;
using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.Domain.Aggregates.UserAggregate
{
    [Table("Users")]
    public partial class User : Entity<Guid>
    {
        public string Email { get; private set; }
        public string Name { get; private set; }
        public string Password { get; private set; }
        public UserRole Role { get; private set; }
    }
}
