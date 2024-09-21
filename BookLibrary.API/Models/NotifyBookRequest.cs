using BookLibrary.API.Services;
using FluentValidation;

namespace BookLibrary.API.Models
{
    public class NotifyBookRequest
    {
        /// <summary>
        /// Book unique ID
        /// </summary>
        /// <example>6e7d48d6-8dd1-494f-a03e-60a698f71b72</example>
        public Guid BookId { get; set; }

        public NotifyBookRequest Validate()
        {
            var requestValidator = new NotifyBookRequestValidator();
            var validationResponse = requestValidator.Validate(this);
            if (!validationResponse.IsValid) throw new ValidationException(validationResponse.ToString(" ~ "));
            ServiceConstants.ValidateId(BookId, nameof(BookId));
            return this;
        }
    }

    public class NotifyBookRequestValidator : AbstractValidator<NotifyBookRequest>
    {
        public NotifyBookRequestValidator()
        {
            RuleFor(x => x.BookId).NotEmpty();
        }
    }
}
