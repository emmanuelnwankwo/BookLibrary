using BookLibrary.Domain.Aggregates.BookAggregate;
using BookLibrary.Domain.Aggregates.UserAggregate;
using BookLibrary.Domain.SeedWork;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrary.Domain.Aggregates.ReservationAggregate
{
    [Table("Reservations")]
    public partial class Reservation : Entity<Guid>
    {
        public Guid BookId { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime ReservedAt { get; private set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; private set; } = DateTime.UtcNow.AddHours(24);
        public bool IsActive { get; private set; } = true;

        public virtual User User { get; private set; }
        public virtual Book Book { get; private set; }
    }
}
