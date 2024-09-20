using BookLibrary.Domain.Aggregates.BookAggregate;
using Microsoft.EntityFrameworkCore;
using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.Infrastructure.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(EFContext dbContext) : base(dbContext)
        {
            
        }

        //public async Task GetBooks()
        //{
        //    var ee = await GetAllAsync(1, 1, null);
        //    //var sqlBuilder = new SqlBuilder();
        //    //var template = sqlBuilder.AddTemplate("SELECT *, COUNT(CustomerId) OVER () as TotalCount FROM [oms].[UpcomingScheduleTemplate] /**where**/ ");
        //    //sqlBuilder = Queryable(sqlBuilder, customerId, requestType, orderBy: orderBy, direction: direction);
        //    //var sql = AddSort(template.RawSql, direction, orderBy);
        //    //sql += ToPaged(pageIndex, pageSize);

        //}
    }
}
