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
        /// <example>test@gmail.com</example>
        public string Email { get; set; }
    }
}
