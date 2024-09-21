using BookLibrary.Domain.Aggregates.ReservationAggregate;

namespace BookLibrary.Test.Domain
{
    [TestFixture]
    public class ReservationTests
    {
        [Test]
        public void Add_ShouldCreateNewReservationWithCorrectProperties()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var bookId = Guid.NewGuid();

            // Act
            var reservation = new Reservation().Add(userId, bookId);

            // Assert
            Assert.That(reservation.UserId, Is.EqualTo(userId));
            Assert.That(reservation.BookId, Is.EqualTo(bookId));
            Assert.IsTrue(reservation.IsActive);  
            Assert.IsNotNull(reservation.ExpiresAt);
        }

        [Test]
        public void EndReservation_ShouldSetExpiresAtToCurrentTimeAndSetIsActiveToFalse()
        {
            // Arrange
            var reservation = new Reservation();

            // Act
            reservation.EndReservation();

            // Assert
            Assert.IsFalse(reservation.IsActive);
            Assert.That((DateTime.UtcNow - reservation.ExpiresAt).TotalSeconds, Is.LessThan(1)); 
        }
    }
}
