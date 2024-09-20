using BookLibrary.Domain.Aggregates.ReservationAggregate;

namespace BookLibrary.Infrastructure.Repositories
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(EFContext dbContext) : base(dbContext)
        {
        }
    }
}
