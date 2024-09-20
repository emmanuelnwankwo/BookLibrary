
using BookLibrary.API.Models;
using BookLibrary.Domain.Aggregates.BookAggregate;

namespace BookLibrary.API.Services.Books
{
    public interface IBookService
    {
        Task<Book> AddBook(AddBookRequest request);
        Task BorrowBook(BorrowBookRequest request);
        Task ReserveBook(ReserveBookRequest request);
        Task ReturnBook(ReturnBookRequest request);
    }
}
