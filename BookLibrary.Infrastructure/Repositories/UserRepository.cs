using BookLibrary.Domain.Aggregates.UserAggregate;

namespace BookLibrary.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(EFContext dbContext) : base(dbContext)
        {

        }
    }
}
