using BookLibrary.API.Models;
using BookLibrary.API.Services.Books;
using BookLibrary.Domain.Aggregates.UserAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService) 
        {
            _bookService = bookService;
        }

        [HttpGet]
        public string Ping()
        {
            return "BookController Up!!!";
        }

        /// <summary>
        /// Add new book to library collection
        /// </summary>
        /// <param name="request"></param>
        /// <returns>success</returns>
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] AddBookRequest request)
        {
           var book = await _bookService.AddBook(request);
            return Ok(book);
        }

        /// <summary>
        /// Get books from library collection
        /// </summary>
        /// <param name="request"></param>
        /// <returns>success</returns>
        //[HttpGet]
        //public async Task<IActionResult> GetBooks([FromQuery] PaginationQuery query)
        //{
        //    var res = await _bookService.GetBooks();
        //    return Ok(res);
        //}

        /// <summary>
        /// Reserve a book of interest from library collection
        /// </summary>
        /// <param name="request"></param>
        /// <returns>success</returns>
        [HttpPost, Route("Reserve")]
        public async Task<IActionResult> ReserveBooks([FromBody] ReserveBookRequest request)
        {
            await _bookService.ReserveBook(request);
            return Ok("Book reserved successfully");
        }

        /// <summary>
        /// Borrows a book from library collection
        /// </summary>
        /// <param name="request"></param>
        /// <returns>success</returns>
        [HttpPost, Route("Borrow")]
        public async Task<IActionResult> BorrowBook([FromBody] BorrowBookRequest request)
        {
            await _bookService.BorrowBook(request);
            return Ok();
        }

        /// <summary>
        /// Return a borrowed book
        /// </summary>
        /// <param name="request"></param>
        /// <returns>success</returns>
        [HttpPost, Route("Return")]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnBookRequest request)
        {
            await _bookService.ReturnBook(request);
            return Ok();
        }

    }
}
