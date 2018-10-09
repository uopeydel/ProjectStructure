using Microsoft.EntityFrameworkCore;
using Pjs1.Common.GenericDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pjs1.DAL.Implementations
{
    public partial class GenericEFRepository : IGenericEFRepository
    {
        public readonly MsSqlGenericDb _msGenericDb;
        public MsSqlGenericDb context() { return _msGenericDb; }
        public GenericEFRepository(MsSqlGenericDb msGenericDb)
        {
            _msGenericDb = msGenericDb;
        }

        public async Task TestGet()
        {
            var iii = await _msGenericDb.Set<Common.DAL.Models.Interlocutor>().ToListAsync();
            var inter = new Common.DAL.Models.Interlocutor { DisplayName = "d", InterlocutorType = Common.DAL.Models.InterlocutorType.None, ProfileImageUrl = "p", StatusUnderName = "ss", TimeZone = "tzx" };
            _msGenericDb.Interlocutor.Add(inter);
            var savere = await _msGenericDb.SaveChangesAsync();
            var interlocutor = await _msGenericDb.Interlocutor.FirstOrDefaultAsync();
            var Contacts = await _msGenericDb.Contact.FirstOrDefaultAsync();

        }

        public virtual async Task<Results<IQueryable<O>>> PagingResultProjectionAsync<DTO, O>(IQueryable<O> query, PagingParameters paging, bool noLimit = false) where O : class
        {
            try
            {
                query = LinqExtensions.OrderBy(query, paging.asc, paging.orderBy);
                paging.top = (paging.top < 0 || paging.top > 5) ? 5 : paging.top;
                int total = 0;
                if (paging.top != 0)
                {
                    paging.pageSize = paging.top;
                    paging.page = 1;
                    total = paging.top;
                    query = query
                        .Take(paging.top);
                }
                else if (noLimit == false)
                {
                    paging.pageSize = (paging.pageSize > 51 || paging.pageSize < 1) ? 20 : paging.pageSize;
                    paging.page = (paging.page < 1) ? 1 : paging.page;
                    total = await query.CountAsync();
                    query = query
                        .Skip(paging.pageSize * (paging.page - 1))
                        .Take(paging.pageSize);
                }
                else
                {
                    total = await query.CountAsync();
                    paging.pageSize = total;
                    paging.page = 1;
                    
                }
                return CreateErrorResponse<IQueryable<O>>("error here" );
                //return CreateSuccessResponse<IQueryable<O>>(query, paging);
            }
            catch (Exception ex)
            {
                //var result = new Results<List<DTO>>();
                //result.errors = new List<string> { ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message };
                //return result;
               return CreateErrorResponse<IQueryable<O>>("error here", ex);
            }
        }

        public Results<TResult> CreateSuccessResponse<TResult>(TResult tresult , PagingInfo pagingInfo )
        {
            var result = new Results<TResult>();
            result.results = tresult;
            result.pageInfo = pagingInfo;
            return result;
        }

        public Results<TResult> CreateErrorResponse<TResult>(string error, Exception exception = null)
        {
            var result = new Results<TResult>();
            result.errors = GetInnerExceptionMessage(exception);
            result.errors.Add(error);
            return result;
        }

        private List<string> GetInnerExceptionMessage(Exception exception)
        {
            var errorTextList = new List<string>();
            var error = GetSubInnerExceptionMessage(exception);
            while (string.IsNullOrEmpty(error))
            {
                errorTextList.Add(error);
            }
            return errorTextList;
        }

        private string GetSubInnerExceptionMessage(Exception exception)
        {
            return exception.InnerException?.Message;
        }

    }


    public interface IGenericEFRepository
    {
        MsSqlGenericDb context();
        Task TestGet();
    }
    public class Results<DTO>
    {
        public DTO results { get; set; }
        public List<string> errors { get; set; }
        public PagingInfo pageInfo { get; set; }
    }

    public class PagingParameters
    {
        public string orderBy { get; set; } = "id";
        public bool asc { get; set; } = true;
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 20;
        public int top { get; set; } = 0;
    }

    public class PagingInfo
    {
        public int total { get; set; }
        public int totalPages { get; set; }
        public int pageSize { get; set; }
        public int currentPage { get; set; }
    }
    public static class LinqExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source,
                     bool asc, params string[] orderByProperties) where T : class
        {
            var command = asc ? "OrderBy" : "OrderByDescending";
            var type = typeof(T);
            var parameter = Expression.Parameter(type, "p");

            var parts = orderByProperties[0].Split('.');

            Expression parent = parameter;

            foreach (var part in parts)
            {
                parent = Expression.Property(parent, part);
            }
            var orderByExpression = Expression.Lambda(parent, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new[] { type, parent.Type },
                source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<T>(resultExpression);
        }
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
