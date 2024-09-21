using BookLibrary.Domain.Aggregates.NotificationAggregate;

namespace BookLibrary.Infrastructure.Repositories
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(EFContext dbContext) : base(dbContext)
        {
        }
    }
}
