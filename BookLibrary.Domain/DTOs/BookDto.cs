using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.Domain.DTOs
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Authors { get; set; }
        public string CoverPictureUrl { get; set; }
        public BookGenre Genre { get; set; }
        public BookStatus Status { get; set; }
        public DateTime DatePublished { get; set; }
    }
}
