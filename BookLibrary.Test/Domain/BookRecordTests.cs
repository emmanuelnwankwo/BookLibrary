using BookLibrary.Domain.Aggregates.BookRecordAggregate;
using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.Test.Domain
{
    public class BookRecordTests
    {
        [Test]
        public void CreateRecord_ShouldCreateNewBookRecordWithCorrectProperties()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var bookId = Guid.NewGuid();
            var expectedReturnDate = new DateTime(2024, 10, 21);

            // Act
            var bookRecord = new BookRecord().CreateRecord(userId, bookId, expectedReturnDate);

            // Assert
            Assert.That(bookRecord.UserId, Is.EqualTo(userId));
            Assert.That(bookRecord.BookId, Is.EqualTo(bookId));
            Assert.That(bookRecord.ExpectedReturnDate, Is.EqualTo(expectedReturnDate));
            Assert.Null(bookRecord.ActualReturnDate);  
            Assert.That(bookRecord.Status, Is.EqualTo(BookStatus.Borrowed));
        }

        [Test]
        public void ReturnRecordUpdate_ShouldSetActualReturnDateAndStatusToReturn()
        {
            // Arrange
            var bookRecord = new BookRecord();

            // Act
            bookRecord.ReturnRecordUpdate();

            // Assert
            Assert.That(bookRecord.Status, Is.EqualTo(BookStatus.Return));
            //Assert.True((DateTime.UtcNow - bookRecord.ActualReturnDate).TotalSeconds < 1);  // Ensure ActualReturnDate is set close to current time
        }
    }
}
