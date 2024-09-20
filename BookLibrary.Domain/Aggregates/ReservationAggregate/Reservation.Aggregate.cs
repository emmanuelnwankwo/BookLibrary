using BookLibrary.Domain.SeedWork;

namespace BookLibrary.Domain.Aggregates.ReservationAggregate
{
    public partial class Reservation : IAggregateRoot
    {
        public Reservation Add(Guid userId, Guid bookId) 
        {
            return new Reservation
            {
                BookId = bookId,
                UserId = userId
            };
        }

        public void EndReservation()
        {
            ExpiresAt = DateTime.UtcNow;
            IsActive = false;
        }
    }
}
