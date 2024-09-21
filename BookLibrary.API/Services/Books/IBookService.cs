
using BookLibrary.API.Models;
using BookLibrary.Domain.Aggregates.BookAggregate;
using BookLibrary.Domain.DTOs;
using BookLibrary.Domain.Shared;

namespace BookLibrary.API.Services.Books
{
    public interface IBookService
    {
        Task<Book> AddBook(AddBookRequest request);
        Task BorrowBook(BorrowBookRequest request);
        Task<PaginatedList<BookDto>> GetBooks(PaginationQuery query);
        Task ReserveBook(ReserveBookRequest request, Guid userId);
        Task ReturnBook(ReturnBookRequest request);
    }
}
