using BookLibrary.Domain.SeedWork;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrary.Domain.Aggregates.NotificationAggregate
{
    [Table("Notifications")]
    public partial class Notification : Entity<Guid>
    {
        public Guid BookId { get; private set; }
        public string UserEmail { get; private set; }
        public DateTime? NotifiedDate { get; private set; }
        public bool IsSent { get; private set; } = false;
    }
}
