using AutoMapper;
using BookLibrary.API.Models;
using BookLibrary.Domain.Aggregates.BookAggregate;
using BookLibrary.Domain.Aggregates.BookRecordAggregate;
using BookLibrary.Domain.Aggregates.ReservationAggregate;
using BookLibrary.Domain.DTOs;
using BookLibrary.Domain.SeedWork;
using BookLibrary.Domain.Shared;
using System.Net;
using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.API.Services.Books
{
    public class BookService : BaseService, IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IBookRecordRepository _bookRecordRepository;
        public BookService(ILogger<BookService> logger, IMapper mapper, 
            IBookRepository bookRepository, IReservationRepository reservationRepository,
            IBookRecordRepository bookRecordRepository) 
            : base(logger, mapper)
        {
            _bookRepository = bookRepository;
            _reservationRepository = reservationRepository;
            _bookRecordRepository = bookRecordRepository;
        }

        public async Task<Book> AddBook(AddBookRequest request)
        {
            var bookExist = await _bookRepository.GetAsync(x => x.Title == request.Title && x.Authors == request.Authors);
            if (bookExist != null) throw new Exception("Book already exist in library collection");

            var bookInst = new Book();
            var bookDto = _mapper.Map<BookDto>(request);
            var book = bookInst.AddBook(bookDto);
            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();
            return book;
        }

        //public async Task<PaginatedList<BookDto>> GetBooks(PaginationQuery query)
        //{
        //    var bookList = await _bookRepository.ListAsync(x => x..Title == "");
        //    var books = _mapper.Map<PaginatedList<BookDto>>(bookList);
        //    return books;
        //}

        //public async Task<PaginatedList<BookDto>> GetBooks(PaginationQuery query)
        //{
        //    var columnValue = new SqlParameter("columnValue", "http://SomeURL");
        //    var bookList = await _bookRepository.GetAllAsync(1, 2, Book);
        //    var books = _mapper.Map<PaginatedList<BookDto>>(bookList);
        //    return books;
        //}

        public async Task ReserveBook(ReserveBookRequest request)
        {
            var userId = new Guid("42dfe4c0-5cb9-43df-8fc8-d05a6bfc0ca0");
            var book = await GetBookById(request.BookId);

            if (book.Status == BookStatus.Borrowed)
            {
                var bookRecord = await _bookRecordRepository.GetAsync(x => x.Id == request.BookId);
                throw new Exception($"Book is currently {book.Status} and will be available on {bookRecord.ExpectedReturnDate.ToString("ddd-MMM-yyyy")}");
            }

            if (book.Status == BookStatus.Reserved)
            {
                throw new Exception($"Book selected book is currently {nameof(BookStatus.Reserved)}");
            }

            book.ReserveBook();

            var reservationInst = new Reservation();
            var reservation = reservationInst.Add(userId, book.Id);

            await _bookRepository.UpdateAsync(book);
            await _reservationRepository.AddAsync(reservation);
            await _reservationRepository.SaveChangesAsync();

        }
        
        public async Task BorrowBook(BorrowBookRequest request)
        {
            var userId = new Guid();
            var book = await GetBookById(request.BookId);
            var reservation = await _reservationRepository.GetAsync(x =>x.Id == request.BookId);

            if (book.Status == BookStatus.Reserved && reservation.UserId != userId)
            {
                throw new Exception($"Book has been {book.Status}");
            }

            if (book.Status == BookStatus.Borrowed)
            {
                var record = await _bookRecordRepository.GetAsync(x => x.Id == request.BookId);
                throw new Exception($"Book selected book is currently {nameof(BookStatus.Borrowed)}. It will be available on {record.ExpectedReturnDate.ToString("ddd-MMM-yyyy")}");
            }

            book.BorrowBook();
            var bookRecordInst = new BookRecord();
            var bookRecord = bookRecordInst.CreateRecord(userId, request.BookId, request.ReturnDate);
            

            await _bookRepository.UpdateAsync(book);
            await _bookRecordRepository.AddAsync(bookRecord);
            await _bookRecordRepository.SaveChangesAsync();
        }
        
        public async Task ReturnBook(ReturnBookRequest request)
        {
            var userId = new Guid();
            var book = await _bookRepository.GetAsync(x => x.Id == request.BookId && x.Status == BookStatus.Borrowed);

            if (book == null)
            {
                throw new Exception("Book not available for the customer");
            }

            var bookRecord = await _bookRecordRepository.GetAsync(x => x.BookId == request.BookId && x.UserId == userId && x.Status == BookStatus.Borrowed);
            bookRecord.ReturnRecordUpdate();
            book.ReturnBook();

            await _bookRepository.UpdateAsync(book);
            await _bookRecordRepository.UpdateAsync(bookRecord);
            await _bookRecordRepository.SaveChangesAsync();
        }

        private async Task<Book> GetBookById(Guid id)
        {
            var book = await _bookRepository.GetAsync(x => x.Id == id);
            if (book == null)
            {
                throw new ArgumentNullException("Book not found.");
            }
            return book;
        }
    }
}
