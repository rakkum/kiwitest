using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BooksApp.Infrastructure
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> SkipAndTake<T>(this IQueryable<T> queryable, ISearchModel sortModel)
        {
            if (sortModel.Skip > 0)
            {
                queryable = queryable.Skip(sortModel.Skip);
            }

            if (sortModel.Take > 0)
            {
                queryable = queryable.Take(sortModel.Take);
            }

            return queryable;
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, ISearchModel sortModel)
        {
            if (string.IsNullOrWhiteSpace(sortModel.SortColumn))
            {
                return queryable;
            }

            var typeOfT = typeof(T);

            var property = typeOfT.GetProperty(sortModel.SortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                return queryable;
            }

            var parameter = Expression.Parameter(typeOfT, "parameter");

            var propertyAccess = Expression.PropertyOrField(parameter, sortModel.SortColumn);

            var orderExpression = Expression.Lambda(propertyAccess, parameter);

            var orderByMethod = sortModel.SortDir == "desc"
                ? "OrderByDescending"
                : "OrderBy";

            var expression = Expression.Call(typeof(Queryable), orderByMethod, new[] {typeOfT, property.PropertyType},
                queryable.Expression, Expression.Quote(orderExpression));
            
            return queryable.Provider.CreateQuery<T>(expression);
        }
    }
}
