using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Application.Common.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Conditional Query Extension to enable search in LINQ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }

        /// <summary>
        /// Take a number of entity based on Condition in LINQ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="query"></param>
        /// <param name="orderBy"></param>
        /// <param name="condition"></param>
        /// <param name="limit"></param>
        /// <param name="orderByDescending"></param>
        /// <returns></returns>
        public static IQueryable<T> TakeIf<T, TKey>(this IQueryable<T> query, Expression<Func<T, TKey>> orderBy, bool condition, int limit, bool orderByDescending = true)
        {
            // It is necessary sort items before it
            query = orderByDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);

            return condition
                ? query.Take(limit)
                : query;
        }

        /// <summary>
        /// Returns Paginated list using LINQ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="query"></param>
        /// <param name="orderBy"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByDescending"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IQueryable<T> PageBy<T, TKey>(this IQueryable<T> query, Expression<Func<T, TKey>> orderBy, int page, int pageSize, bool orderByDescending = true)
        {
            const int defaultPageNumber = 1;

            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            // Check if the page number is greater then zero - otherwise use default page number
            if (page <= 0)
            {
                page = defaultPageNumber;
            }

            // It is necessary sort items before it
            query = orderByDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
