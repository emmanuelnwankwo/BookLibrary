using FluentValidation;

namespace BookLibrary.API.Models.Users
{
    public class AddUserRequest
    {
        /// <summary>
        /// User email 
        /// </summary>
        /// <example>john.doe@example.com</example>
        public string Email { get; set; }

        /// <summary>
        /// User full name
        /// </summary>
        /// <example>John Doe</example>
        public string Name { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        /// <example>Password123</example>
        public string Password { get; set; }

        public AddUserRequest Validate()
        {
            var requestValidator = new AddUserRequestValidator();
            var validationResponse = requestValidator.Validate(this);
            if (!validationResponse.IsValid) throw new ValidationException(validationResponse.ToString(" ~ "));
            return this;
        }
    }

    public class AddUserRequestValidator : AbstractValidator<AddUserRequest>
    {
        public AddUserRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Email).EmailAddress().MaximumLength(50);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(50);
        }
    }
}
