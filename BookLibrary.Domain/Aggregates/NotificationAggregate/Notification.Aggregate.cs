using BookLibrary.Domain.DTOs;
using BookLibrary.Domain.SeedWork;

namespace BookLibrary.Domain.Aggregates.NotificationAggregate
{
    public partial class Notification : IAggregateRoot
    {
        public Notification Create(NotificationDto notification) 
        {
            return new Notification
            {
                UserEmail = notification.UserEmail,
                BookId = notification.BookId
            };
        }
    }
}
