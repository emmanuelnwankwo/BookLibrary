using FluentValidation;

namespace BookLibrary.API.Models
{
    public class ReserveBookRequest
    {
        /// <summary>
        /// Book unique ID
        /// </summary>
        /// <example>6e7d48d6-8dd1-494f-a03e-60a698f71b72</example>
        public Guid BookId { get; set; }

        public ReserveBookRequest Validate()
        {
            var requestValidator = new ReserveBookRequestValidator();
            var validationResponse = requestValidator.Validate(this);
            if (!validationResponse.IsValid) throw new ValidationException(validationResponse.ToString(" ~ "));
            return this;
        }
    }

    public class ReserveBookRequestValidator : AbstractValidator<ReserveBookRequest>
    {
        public ReserveBookRequestValidator()
        {
            RuleFor(x => x.BookId).NotEmpty();
            //RuleFor(x => x.BookId).Must(x => ServiceConstants.CheckGuidValue(x)).WithMessage("Invalid bookId");
        }
    }
}
