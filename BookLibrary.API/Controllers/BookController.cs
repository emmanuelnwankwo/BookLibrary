using BookLibrary.API.Models;
using BookLibrary.API.Services;
using BookLibrary.API.Services.Books;
using BookLibrary.Domain.Aggregates.BookAggregate;
using BookLibrary.Domain.DTOs;
using BookLibrary.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BookLibrary.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController, Authorize]
    public class BookController : BaseController
    {
        private readonly IBookService _bookService;
        public BookController(ILogger<BookController> logger, IBookService bookService): base(logger)
        {
            _bookService = bookService;
        }

        
        /// <summary>
        /// Add new book to library collection
        /// </summary>
        /// <param name="request"></param>
        /// <returns>success</returns>
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(ServiceResponse))]
        [ProducesResponseType(400, Type = typeof(ServiceResponse))]
        [ProducesResponseType(404, Type = typeof(ServiceResponse))]
        [ProducesResponseType(500, Type = typeof(ServiceResponse))]
        public async Task<IActionResult> AddBook([FromBody] AddBookRequest request)
        {
            var book = await _bookService.AddBook(request.Validate());
            return Ok(new ServiceResponse<Book>("successfully added new book to library collection") { Data = book});
        }

        /// <summary>
        /// Get books from library collection
        /// </summary>
        /// <param name="request"></param>
        /// <returns>success</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(PaginatedList<BookDto>))]
        [ProducesResponseType(400, Type = typeof(ServiceResponse))]
        [ProducesResponseType(404, Type = typeof(ServiceResponse))]
        [ProducesResponseType(500, Type = typeof(ServiceResponse))]
        public async Task<IActionResult> GetBooks([FromQuery] PaginationQuery query)
        {
            var books = await _bookService.GetBooks(query);
            return Ok(books);
        }

        /// <summary>
        /// Reserve a book of interest from library collection
        /// </summary>
        /// <param name="request"></param>
        /// <returns>success</returns>
        [HttpPost]
        [Authorize(Roles = "User")]
        [ProducesResponseType(200, Type = typeof(ServiceResponse))]
        [ProducesResponseType(400, Type = typeof(ServiceResponse))]
        [ProducesResponseType(404, Type = typeof(ServiceResponse))]
        [ProducesResponseType(500, Type = typeof(ServiceResponse))]
        public async Task<IActionResult> ReserveBook([FromBody] ReserveBookRequest request)
        {
            await _bookService.ReserveBook(request.Validate(), UserId);
            return Ok(new ServiceResponse("Book reserved successfully"));
        }

        /// <summary>
        /// Borrows a book from library collection
        /// </summary>
        /// <param name="request"></param>
        /// <returns>success</returns>
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(ServiceResponse))]
        [ProducesResponseType(400, Type = typeof(ServiceResponse))]
        [ProducesResponseType(404, Type = typeof(ServiceResponse))]
        [ProducesResponseType(500, Type = typeof(ServiceResponse))]
        public async Task<IActionResult> BorrowBook([FromBody] BorrowBookRequest request)
        {
            await _bookService.BorrowBook(request.Validate());
            return Ok(new ServiceResponse("Book was borrowed successfully"));
        }

        /// <summary>
        /// Return a borrowed book
        /// </summary>
        /// <param name="request"></param>
        /// <returns>success</returns>
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(ServiceResponse))]
        [ProducesResponseType(400, Type = typeof(ServiceResponse))]
        [ProducesResponseType(404, Type = typeof(ServiceResponse))]
        [ProducesResponseType(500, Type = typeof(ServiceResponse))]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnBookRequest request)
        {
            await _bookService.ReturnBook(request.Validate());
            return Ok(new ServiceResponse("Book is now available to be reserved and borrowed by another user"));
        }

    }
}
