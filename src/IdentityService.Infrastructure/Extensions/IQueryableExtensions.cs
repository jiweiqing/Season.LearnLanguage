using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Infrastructure
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// where if
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="istrue"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool istrue, Expression<Func<TSource, bool>> predicate)
        {
            if (istrue)
            {
                source = source.Where(predicate);
            }
            return source;
        }

        public static IQueryable<TSource> WhereIf2<TSource>(this IQueryable<TSource> source, bool istrue, Func<TSource, bool> predicate)
        {
            if (istrue)
            {
                var s = source.AsEnumerable().Where(predicate);
            }
            return source;
        }
    }
}
