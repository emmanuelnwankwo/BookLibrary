using BookLibrary.API.Services;
using FluentValidation;

namespace BookLibrary.API.Models
{
    public class BorrowBookRequest
    {
        /// <summary>
        /// Book unique ID
        /// </summary>
        /// <example>6e7d48d6-8dd1-494f-a03e-60a698f71b72</example>
        public Guid BookId { get; set; }

        /// <summary>
        /// Customer email address
        /// </summary>
        /// <example>john.doe@example.com</example>
        public string Email { get; set; }

        /// <summary>
        /// Expected book return date
        /// </summary>
        /// <example>2024-09-24</example>
        public DateTime ReturnDate { get; set; }

        public BorrowBookRequest Validate()
        {
            var requestValidator = new BorrowBookRequestValidator();
            var validationResponse = requestValidator.Validate(this);
            if (!validationResponse.IsValid) throw new ValidationException(validationResponse.ToString(" ~ "));
            return this;
        }
    }

    public class BorrowBookRequestValidator : AbstractValidator<BorrowBookRequest>
    {
        public BorrowBookRequestValidator()
        {
            RuleFor(x => x.BookId).NotEmpty();
            //RuleFor(x => x.BookId).Must(x => ServiceConstants.CheckGuidValue(x)).WithMessage("Invalid bookId");
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.ReturnDate).NotEmpty();
            RuleFor(x => x.ReturnDate).GreaterThan(DateTime.Now);
        }
    }
}
