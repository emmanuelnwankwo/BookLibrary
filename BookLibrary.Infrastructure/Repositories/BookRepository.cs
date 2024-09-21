using BookLibrary.Domain.Aggregates.BookAggregate;

namespace BookLibrary.Infrastructure.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(EFContext dbContext) : base(dbContext)
        {
            
        }

    }
}
