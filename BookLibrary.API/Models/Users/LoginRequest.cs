using FluentValidation;

namespace BookLibrary.API.Models.Users
{
    public class LoginRequest
    {
        /// <summary>
        /// User email 
        /// </summary>
        /// <example>john.doe@example.com</example>
        public string Email { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        /// <example>Password123</example>
        public string Password { get; set; }

        public LoginRequest Validate()
        {
            var requestValidator = new LoginRequestValidator();
            var validationResponse = requestValidator.Validate(this);
            if (!validationResponse.IsValid) throw new ValidationException(validationResponse.ToString(" ~ "));
            return this;
        }
    }

    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email).EmailAddress().MaximumLength(50);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(50);
        }
    }
}
