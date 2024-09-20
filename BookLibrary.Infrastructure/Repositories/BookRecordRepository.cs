using BookLibrary.Domain.Aggregates.BookRecordAggregate;

namespace BookLibrary.Infrastructure.Repositories
{
    public class BookRecordRepository : BaseRepository<BookRecord>, IBookRecordRepository
    {
        public BookRecordRepository(EFContext dbContext) : base(dbContext)
        {
        }
    }
}
