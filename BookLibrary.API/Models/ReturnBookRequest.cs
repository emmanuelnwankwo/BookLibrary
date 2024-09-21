using FluentValidation;

namespace BookLibrary.API.Models
{
    public class ReturnBookRequest
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

        public ReturnBookRequest Validate()
        {
            var requestValidator = new ReturnBookRequestValidator();
            var validationResponse = requestValidator.Validate(this);
            if (!validationResponse.IsValid) throw new ValidationException(validationResponse.ToString(" ~ "));
            return this;
        }
    }

    public class ReturnBookRequestValidator : AbstractValidator<ReturnBookRequest>
    {
        public ReturnBookRequestValidator()
        {
            RuleFor(x => x.BookId).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
