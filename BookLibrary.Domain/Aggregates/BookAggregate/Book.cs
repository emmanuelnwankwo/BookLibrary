using BookLibrary.Domain.SeedWork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.Domain.Aggregates.BookAggregate
{
    [Table("Books")]
    public partial class Book : Entity<Guid>
    {
        [StringLength(500)]
        public string Title { get; private set; }
        [StringLength(2000)]
        public string Description { get; private set; }
        [StringLength(500)]
        public string Authors { get; private set; }
        [StringLength(2000)]
        public string CoverPictureUrl { get; private set; }
        public DateTime DatePublished { get; private set; }
        public BookStatus Status { get; private set; }
        public BookGenre Genre { get; private set; }
    }
}
