using BookLibrary.Domain.SeedWork;
using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.Domain.Aggregates.BookRecordAggregate
{
    public partial class BookRecord : IAggregateRoot
    {
        public BookRecord CreateRecord(Guid userId, Guid bookId, DateTime expectedReturnDate) 
        {
            return new BookRecord
            {
                UserId = userId,
                BookId = bookId,
                ExpectedReturnDate = expectedReturnDate
            };
        }

        public void ReturnRecordUpdate()
        {
            ActualReturnDate = DateTime.UtcNow;
            Status = BookStatus.Return;
        }
    }
}
