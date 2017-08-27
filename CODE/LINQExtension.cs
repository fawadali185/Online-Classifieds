using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ListHell.CODE
{
    public static class LINQExtension
    {
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
        {
            return items.GroupBy(property).Select(x => x.First());
        }

        public static IQueryable<T> DistinctBy<T, TKey>(this IQueryable<T> items, Expression<Func<T, TKey>> property)
        {
            return items.GroupBy(property).Select(x => x.FirstOrDefault());
        }
    }

}