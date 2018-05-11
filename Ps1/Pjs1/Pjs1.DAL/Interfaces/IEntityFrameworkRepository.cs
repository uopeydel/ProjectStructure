using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pjs1.DAL.Interfaces
{
    public interface IEntityFrameworkRepository<T, TContext> where T : class where TContext : DbContext
    {
        IQueryable<T> GetAll(bool tracking = true);
        IQueryable<T> GetAll(bool tracking = true, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate, bool tracking = true);

        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, bool tracking = true);

        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, bool tracking = true,
            params Expression<Func<T, object>>[] includeProperties);

        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddAsync(IEnumerable<T> entity);
        void Delete(T entity);
        void Delete(List<T> entity);
        void Update(T entity);
        void UpdateSpecficProperty<TProperty>(T entity, params Expression<Func<T, TProperty>>[] properties);
        Task<int> CountAsync();
        int SaveChanges();

    }
}
