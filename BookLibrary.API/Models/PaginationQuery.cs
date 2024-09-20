using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.API.Models
{
    public class PaginationQuery
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public OrderBy OrderBy { get; set; }
    }
}
