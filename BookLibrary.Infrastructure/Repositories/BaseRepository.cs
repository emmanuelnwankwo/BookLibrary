using BookLibrary.Domain.SeedWork;
using BookLibrary.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static BookLibrary.Domain.Shared.Enums;

namespace BookLibrary.Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly EFContext _dbContext;
        private Dictionary<string, object> sqlParameters;


        public BaseRepository(EFContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public Task<bool> DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            return Task.FromResult(true);
        }

        public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.FirstOrDefaultAsync(expression);
        }

        public Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.Where(expression).ToListAsync();
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            return Task.FromResult(entity);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return _dbContext.SaveChangesAsync();
        }

        private async Task<PaginatedList<TEntity>> GetAllAsync<T>(int pageIndex, int pageSize, Expression<Func<TEntity, T>> keySelector,
            Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = FilterQuery(keySelector, predicate, orderBy, includeProperties);
            var total = await entities.CountAsync();
            entities = entities.Paginate(pageIndex, pageSize);
            var list = await entities.ToListAsync();
            return list.ToPaginatedList(pageIndex, pageSize, total);
        }

        private IQueryable<TEntity> FilterQuery<T>(Expression<Func<TEntity, T>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy,
               Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            entities = (predicate != null) ? entities.Where(predicate) : entities;
            entities = (orderBy == OrderBy.Ascending)
                ? entities.OrderBy(keySelector)
                : entities.OrderByDescending(keySelector);
            return entities;
        }

        private IQueryable<TEntity> IncludeProperties(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> entities = _dbSet;

            foreach (var includeProperty in includeProperties)
            {
                entities = entities.Include(includeProperty);
            }
            return entities;
        }

        public async Task<PaginatedList<TEntity>> GetAllAsync<T>(int pageIndex, int pageSize, Expression<Func<TEntity, T>> keySelector, OrderBy orderBy = OrderBy.Ascending)
        {
            return await GetAllAsync(pageIndex, pageSize, keySelector, null, orderBy);
        }

        //public async Task<PaginatedList<TEntity>> UpdateAsync(int pageIndex, int pageSize, OrderBy orderBy, Expression<Func<TEntity, object>>[] includeProperties)
        //{

        //    var entities = IncludeProperties(includeProperties);
        //    entities = (predicate != null) ? entities.Where(predicate) : entities;
        //    entities = (orderBy == OrderBy.Ascending)
        //        ? entities.OrderBy(keySelector)
        //        : entities.OrderByDescending(keySelector);


        //    var total = await entities.CountAsync();
        //    entities = entities.Paginate(pageIndex, pageSize);
        //    var list = await entities.ToListAsync();
        //    return list.ToPaginatedList(pageIndex, pageSize, total);
        //}
    }
}
