using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pjs1.DAL.PostgreSQL.Interfaces
{
    public interface IEntityFrameworkRepository<T, TContext> where T : class where TContext : DbContext
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);

        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties);

        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddAsync(IEnumerable<T> entity);
        void Delete(T entity);
        void Delete(List<T> entity);
        void Update(T entity);
        Task<int> CountAsync();
        int SaveChanges();

    }
}
