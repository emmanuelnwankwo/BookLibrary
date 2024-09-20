using BookLibrary.Domain.DTOs;
using BookLibrary.Domain.SeedWork;
using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.Domain.Aggregates.BookAggregate
{
    public partial class Book : IAggregateRoot
    {
        //public Book() { }
        public Book AddBook(BookDto book)
        {
            return new Book
            {
                Title = book.Title,
                Description = book.Description,
                CoverPictureUrl = book.CoverPictureUrl,
                Authors = book.Authors,
                Genre = book.Genre,
                Status = BookStatus.Available,
                DatePublished = book.DatePublished
            };
        }

        public void Update(BookDto book)
        {
            Title = book.Title;
            Description = book.Description;
            CoverPictureUrl = book.CoverPictureUrl;
            Authors = book.Authors;
            Genre = book.Genre;
            Status = book.Status;            
            DatePublished = book.DatePublished;
            UpdatedAt = DateTime.UtcNow.AddHours(1);
        }
        
        public void ReserveBook()
        {
            Status = BookStatus.Reserved;
        }
        
        public void BorrowBook()
        {
            Status = BookStatus.Borrowed;
        }
        
        public void ReturnBook()
        {
            Status = BookStatus.Available;
        }
    }
}
