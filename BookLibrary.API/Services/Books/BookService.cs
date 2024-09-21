using AutoMapper;
using BookLibrary.API.Models;
using BookLibrary.API.Services.Users;
using BookLibrary.Domain.Aggregates.BookAggregate;
using BookLibrary.Domain.Aggregates.BookRecordAggregate;
using BookLibrary.Domain.Aggregates.NotificationAggregate;
using BookLibrary.Domain.Aggregates.ReservationAggregate;
using BookLibrary.Domain.DTOs;
using BookLibrary.Domain.Shared;
using BookLibrary.Infrastructure.Repositories;
using System.Linq.Expressions;
using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.API.Services.Books
{
    public class BookService : BaseService, IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IBookRecordRepository _bookRecordRepository;
        private readonly IUserService _userService;
        private readonly INotificationRepository _notificationRepository;
        public BookService(ILogger<BookService> logger, IMapper mapper, 
            IBookRepository bookRepository, IReservationRepository reservationRepository,
            IBookRecordRepository bookRecordRepository, IUserService userService,
            INotificationRepository notificationRepository) 
            : base(logger, mapper)
        {
            _bookRepository = bookRepository;
            _reservationRepository = reservationRepository;
            _bookRecordRepository = bookRecordRepository;
            _userService = userService;
            _notificationRepository = notificationRepository;
        }

        public async Task<Book> AddBook(AddBookRequest request)
        {
            var bookExist = await _bookRepository.GetAsync(x => x.Title == request.Title && x.Authors == request.Authors);
            if (bookExist != null) throw new ArgumentNullException("Book already exist in library collection");

            var bookInst = new Book();
            var bookDto = _mapper.Map<BookDto>(request);
            var book = bookInst.AddBook(bookDto);
            await _bookRepository.InsertAsync(book);
            await _bookRepository.SaveChangesAsync();
            return book;
        }

        public async Task<PaginatedList<BookDto>> GetBooks(PaginationQuery query)
        {
            Expression<Func<Book, bool>>? predicate = null;
            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                predicate = s => s.Title.ToLower().Contains(query.SearchTerm.ToLower());
            }

            var bookList = await _bookRepository.GetAllByPaginationAsync(query.PageIndex, query.PageSize, x => x.Title, predicate, query.OrderBy);
            var books = _mapper.Map<PaginatedList<BookDto>>(bookList);
            return books;
        }

        public async Task ReserveBook(ReserveBookRequest request, Guid userId)
        {
            var book = await GetBookById(request.BookId);

            if (book.Status == BookStatus.Borrowed)
            {
                var bookRecord = await _bookRecordRepository.GetAsync(x => x.Id == request.BookId);
                throw new ArgumentException($"Book is currently {book.Status} and will be available on {bookRecord.ExpectedReturnDate.ToString("ddd-MMM-yyyy")}");
            }

            if (book.Status == BookStatus.Reserved)
            {
                throw new ArgumentException($"Book selected book is currently {nameof(BookStatus.Reserved)}");
            }

            book.ReserveBook();

            var reservationInst = new Reservation();
            var reservation = reservationInst.Add(userId, book.Id);

            await _bookRepository.UpdateAsync(book);
            await _reservationRepository.InsertAsync(reservation);
            await _reservationRepository.SaveChangesAsync();

        }
        
        public async Task BorrowBook(BorrowBookRequest request)
        {
            var book = await GetBookById(request.BookId);
            var reservation = await _reservationRepository.GetAsync(x =>x.BookId == request.BookId && x.IsActive);
            var user = await _userService.GetUserByEmail(request.Email);

            if (book.Status == BookStatus.Available)
            {
                throw new ArgumentException($"{user.Name} did not made a reservation for the book");
            }

            if (book.Status == BookStatus.Reserved && reservation == null)
            {
                throw new ArgumentException($"The book was reserved by another user");
            }

            if (book.Status == BookStatus.Borrowed)
            {
                var record = await _bookRecordRepository.GetAsync(x => x.BookId == request.BookId);
                throw new ArgumentException($"Book selected book is currently {nameof(BookStatus.Borrowed)}. It will be available on {record.ExpectedReturnDate.ToString("ddd-MMM-yyyy")}");
            }
            reservation.EndReservation();
            book.BorrowBook();
            var bookRecordInst = new BookRecord();
            var bookRecord = bookRecordInst.CreateRecord(user.Id, request.BookId, request.ReturnDate);
            

            await _bookRepository.UpdateAsync(book);
            await _bookRecordRepository.InsertAsync(bookRecord);
            await _bookRecordRepository.SaveChangesAsync();
        }
        
        public async Task ReturnBook(ReturnBookRequest request)
        {
            var book = await _bookRepository.GetAsync(x => x.Id == request.BookId && x.Status == BookStatus.Borrowed);

            if (book == null)
            {
                throw new ArgumentNullException("Book not available for the customer");
            }
            var user = await _userService.GetUserByEmail(request.Email);
            var bookRecord = await _bookRecordRepository.GetAsync(x => x.BookId == request.BookId && x.UserId == user.Id && x.Status == BookStatus.Borrowed);
            if (bookRecord == null)
            {
                throw new ArgumentNullException("Book not part of customer borrowed collection");
            }

            bookRecord.ReturnRecordUpdate();
            book.ReturnBook();

            await _bookRepository.UpdateAsync(book);
            await _bookRecordRepository.UpdateAsync(bookRecord);
            await _bookRecordRepository.SaveChangesAsync();
        }

        public async Task NotifyAboutBook(NotifyBookRequest request, Guid userId, string userEmail)
        {
            var notificationRecord = await _notificationRepository.GetAsync(x => x.BookId == request.BookId && x.UserEmail == userEmail && !x.IsSent);
            if (notificationRecord != null)
            {
                throw new ArgumentException("You have already requested to be notified for the book!");
            }

            var book = await _bookRepository.GetAsync(x => x.Id == request.BookId && x.Status == BookStatus.Available);

            if (book != null)
            {
                throw new ArgumentException($"'{book.Title}' is available. Please make a reservation");
            }

            var notificationInst = new Notification();
            var notificationDto = new NotificationDto
            {
                UserEmail = userEmail,
                BookId = request.BookId,
            };
            var notification = notificationInst.Create(notificationDto);

            await _notificationRepository.InsertAsync(notification);
            await _notificationRepository.SaveChangesAsync();
        }

        private async Task<Book> GetBookById(Guid id)
        {
            var book = await _bookRepository.GetAsync(x => x.Id == id);
            return book ?? throw new ArgumentNullException("Book not found.");
        }
    }
}
