using FluentValidation;
using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.API.Models
{
    public class AddBookRequest
    {
        /// <summary>
        /// Book unique title
        /// </summary>
        /// <example>The Psychology of Money</example>
        public string Title { get; set; }

        /// <summary>
        /// Short description of the book
        /// </summary>
        /// <example>Doing well with money isn't necessarily about what you know. It's about how you behave. And behavior is hard to teach, even to really smart people. Money--investing, personal finance, and business decisions--is typically taught as a math-based field, where data and formulas tell us exactly what to do. But in the real world people don't make financial decisions on a spreadsheet. They make them at the dinner table, or in a meeting room, where personal history, your own unique view of the world, ego, pride, marketing, and odd incentives are scrambled together. In The Psychology of Money, award-winning author Morgan Housel shares 19 short stories exploring the strange ways people think about money and teaches you how to make better sense of one of life's most important topics.</example>
        public string Description { get; set; }

        /// <summary>
        /// Book author(s) name
        /// </summary>
        /// <example>Morgan Housel</example>
        public string Authors { get; set; }

        /// <summary>
        /// Book cover image URL
        /// </summary>
        /// <example>https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1581527774i/41881472.jpg</example>
        public string CoverPictureUrl { get; set; }

        /// <summary>
        /// Book genre
        /// </summary>
        /// <example>0</example>
        public BookGenre Genre { get; set; }
        
        /// <summary>
        /// Book published date
        /// </summary>
        /// <example>2020-01-01</example>
        public DateTime DatePublished { get; set; }
    }

    public class AddBookRequestValidator : AbstractValidator<AddBookRequest>
    {
        public AddBookRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Title).MaximumLength(500);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Authors).NotEmpty();
            RuleFor(x => x.CoverPictureUrl).NotEmpty();
            RuleFor(x => x.CoverPictureUrl).MaximumLength(2000);
            RuleFor(x => x.Genre).IsInEnum().WithMessage("Book Genre is required");
            RuleFor(x => x.DatePublished).NotEmpty().WithMessage("DatePublished is required");
        }
    }
}
