using BookLibrary.Domain.Shared;
using System.Linq.Expressions;

namespace BookLibrary.Domain.SeedWork
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        //IUnitOfWork UnitOfWork { get; }
        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<bool> DeleteAsync(TEntity entity);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression);

        Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> expression);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<PaginatedList<TEntity>> GetAllAsync<T>(int pageIndex, int pageSize, Expression<Func<TEntity, T>> keySelector, Enums.OrderBy orderBy = Enums.OrderBy.Ascending);
    }
}
