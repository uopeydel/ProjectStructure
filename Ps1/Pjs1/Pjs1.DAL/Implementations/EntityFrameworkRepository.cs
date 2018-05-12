using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Pjs1.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Pjs1.DAL.Implementations
{
    public class EntityFrameworkRepository<T, TContext> : IEntityFrameworkRepository<T, TContext> where T : class where TContext : DbContext
    {
        protected readonly DbContext DbContext;
        private readonly IServiceProvider _serviceProvider;
        public EntityFrameworkRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            DbContext = GetConnection();
        }

        private DbContext GetConnection()
        {
            var parameter = _serviceProvider.GetRequiredService<DbContextOptions<TContext>>();
            return (TContext)Activator.CreateInstance(typeof(TContext), new object[] { parameter });
        }

        public async Task<int> CountAsync()
        {
            return await DbContext.Set<T>().CountAsync();
        }

        public virtual void Delete(T entity)
        {
            DbContext.Set<T>().Remove(entity);
        }

        public virtual void Delete(List<T> entity)
        {
            DbContext.Set<T>().RemoveRange(entity);
        }

        public virtual IQueryable<T> GetAll(bool tracking = true)
        {
            if (tracking)
            {
                return DbContext.Set<T>().AsQueryable();
            }
            else
            {
                return DbContext.Set<T>().AsNoTracking().AsQueryable();
            }
        }

        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>> predicate, bool tracking = true)
        {
            if (tracking)
            {
                return DbContext.Set<T>().Where(predicate).AsQueryable();
            }
            else
            {
                return DbContext.Set<T>().AsNoTracking().Where(predicate).AsQueryable();
            }
        }

        public IQueryable<T> GetAll(bool tracking = true, params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> query = DbContext.Set<T>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            if (tracking)
            {
                return query.AsQueryable();
            }
            else
            {
                return query.AsNoTracking().AsQueryable();
            }
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, bool tracking = true)
        {
            if (tracking)
            {
                return await DbContext.Set<T>().FirstOrDefaultAsync(predicate);
            }
            else
            {
                return await DbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
            }
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, bool tracking = true,
            params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = DbContext.Set<T>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            if (tracking)
            {
                return await query.Where(predicate).FirstOrDefaultAsync();
            }
            else
            {
                return await query.Where(predicate).AsNoTracking().FirstOrDefaultAsync();
            }
        }

        public async Task<T> AddAsync(T entity)
        {
            await DbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> AddAsync(IEnumerable<T> entity)
        {
            var addAsync = entity as T[] ?? entity.ToArray();
            await DbContext.Set<T>().AddRangeAsync(addAsync);
            return addAsync;
        }

        public void Update(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateSpecficProperty<TProperty>(T entity, params Expression<Func<T, TProperty>>[] properties)
        {
            if (properties != null && properties.Length > 0)
            {
                DbContext.Entry(entity).State = EntityState.Detached;
                DbContext.Attach(entity);
                foreach (var prop in properties)
                {
                    DbContext.Entry(entity).Property(prop).IsModified = true;
                }
                return;
            }
            DbContext.Entry(entity).State = EntityState.Modified;

        }

        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }
    }
}
