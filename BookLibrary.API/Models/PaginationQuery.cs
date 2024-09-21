using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.API.Models
{
    public class PaginationQuery
    {
        /// <summary>
        /// Page size
        /// </summary>
        /// <example>10</example>
        public int PageSize { get; set; }

        /// <summary>
        /// Page index
        /// </summary>
        /// <example>1</example>
        public int PageIndex { get; set; }

        /// <summary>
        /// Order by book title
        /// </summary>
        /// <example></example>
        public OrderBy OrderBy { get; set; } = OrderBy.Ascending;

        /// <summary>
        /// Search book by title
        /// </summary>
        /// <example></example>
        public string? Search { get; set; }
    }
}
