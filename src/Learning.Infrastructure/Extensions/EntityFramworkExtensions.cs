using Learning.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Infrastructure
{
    public static class EntityFramworkExtensions
    {
        /// <summary>
        /// where if
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate) where TSource : class
        {
            if (condition && predicate != null)
            {
                return source.Where(predicate);
            }
            return source;
        }

        /// <summary>
        /// include if
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TProperty">属性</typeparam>
        /// <param name="source">query</param>
        /// <param name="condition">条件</param>
        /// <param name="navigationPropertyPath">要Include的表达式</param>
        /// <returns></returns>
        public static IQueryable<TEntity> IncludeIf<TEntity, TProperty>(
            this IQueryable<TEntity> source, bool condition, Expression<Func<TEntity, TProperty>> navigationPropertyPath) where TEntity : class
        {
            if (condition && navigationPropertyPath != null)
            {
                return source.Include(navigationPropertyPath);
            }

            return source;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> PageIf<TEntity>(this IQueryable<TEntity> source, bool condition, PagedInput input)
        {
            // TODO: 默认排序
            return condition ? source.Skip(input.SkipCount).Take(input.MaxResultCount) : source;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> Page<TEntity>(this IQueryable<TEntity> source, PagedInput input)
        {
            // TODO: 默认排序
            return  source.Skip(input.SkipCount).Take(input.MaxResultCount);
        }
    }
}
