using BookLibrary.Domain.Aggregates.BookAggregate;
using BookLibrary.Domain.Aggregates.UserAggregate;
using BookLibrary.Domain.SeedWork;
using System.ComponentModel.DataAnnotations.Schema;
using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.Domain.Aggregates.BookRecordAggregate
{
    [Table("BookRecords")]
    public partial class BookRecord : Entity<Guid>
    {
        public Guid BookId { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime ExpectedReturnDate { get; private set; }
        public DateTime? ActualReturnDate { get; private set; }
        public BookStatus Status { get; private set; } = BookStatus.Borrowed;

        public virtual User User { get; private set; }
        public virtual Book Book { get; private set; }
    }
}
