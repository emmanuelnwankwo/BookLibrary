using BookLibrary.Domain.Aggregates.BookAggregate;
using BookLibrary.Domain.Aggregates.ReservationAggregate;
using BookLibrary.Domain.DTOs;
using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.Test.Domain
{
    [TestFixture]

    public class BookTests
    {
        [Test]
        public void AddBook_ShouldCreateNewBookWithCorrectProperties()
        {
            // Arrange
            var bookDto = new BookDto
            {
                Title = "Test Title",
                Description = "Test Description",
                CoverPictureUrl = "http://example.com/cover.jpg",
                Authors = "Author 1, Author 2",
                Genre = BookGenre.Business,
                DatePublished = new DateTime(2020, 1, 1)
            };

            // Act
            var book = new Book().AddBook(bookDto);

            // Assert
            Assert.That(book.Title, Is.EqualTo(bookDto.Title));
            Assert.That(book.Description, Is.EqualTo(bookDto.Description));
            Assert.AreEqual(bookDto.CoverPictureUrl, book.CoverPictureUrl);
            Assert.AreEqual(bookDto.Authors, book.Authors);
            Assert.AreEqual(bookDto.Genre, book.Genre);
            Assert.AreEqual(BookStatus.Available, book.Status);
            Assert.AreEqual(bookDto.DatePublished, book.DatePublished);
        }

        [Test]
        public void Update_ShouldUpdateBookPropertiesCorrectly()
        {
            // Arrange
            var book = new Book();
            var bookDto = new BookDto
            {
                Title = "Updated Title",
                Description = "Updated Description",
                CoverPictureUrl = "http://example.com/updatedcover.jpg",
                Authors = "Updated Author",
                Genre = BookGenre.Business,
                Status = BookStatus.Borrowed,
                DatePublished = new DateTime(2021, 2, 2)
            };

            // Act
            book.Update(bookDto);

            // Assert
            Assert.AreEqual(bookDto.Title, book.Title);
            Assert.AreEqual(bookDto.Description, book.Description);
            Assert.AreEqual(bookDto.CoverPictureUrl, book.CoverPictureUrl);
            Assert.AreEqual(bookDto.Authors, book.Authors);
            Assert.AreEqual(bookDto.Genre, book.Genre);
            Assert.AreEqual(BookStatus.Borrowed, book.Status);
            Assert.AreEqual(bookDto.DatePublished, book.DatePublished);
            Assert.That((DateTime.UtcNow.AddHours(1) - book.UpdatedAt.Value).TotalSeconds, Is.LessThan(1));
        }

        [Test]
        public void ReserveBook_ShouldSetStatusToReserved()
        {
            // Arrange
            var book = new Book();

            // Act
            book.ReserveBook();

            // Assert
            Assert.That(book.Status, Is.EqualTo(BookStatus.Reserved));
        }

        [Test]
        public void BorrowBook_ShouldSetStatusToBorrowed()
        {
            // Arrange
            var book = new Book();

            // Act
            book.BorrowBook();

            // Assert
            Assert.That(book.Status, Is.EqualTo(BookStatus.Borrowed));
        }

        [Test]
        public void ReturnBook_ShouldSetStatusToAvailable()
        {
            // Arrange
            var book = new Book();

            // Act
            book.ReturnBook();

            // Assert
            Assert.That(book.Status, Is.EqualTo(BookStatus.Available));
        }
    }
}