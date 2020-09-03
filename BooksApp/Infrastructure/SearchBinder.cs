using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LinqKit;

namespace BooksApp.Infrastructure
{
    public class SearchBinder<TSearchModel, TModel>
    {
        private readonly List<Tuple<PropertyInfo, Expression<Func<TModel, TSearchModel, bool>>>> _propertyToPredicate = new List<Tuple<PropertyInfo, Expression<Func<TModel, TSearchModel, bool>>>>();
        
        public SearchBinder<TSearchModel, TModel> AddSearchMapping<TValue>(Expression<Func<TSearchModel, TValue>> searchModelSelector, Expression<Func<TModel, TSearchModel, bool>> predicate)
        {
            var sortModelProperty = GetProperty(searchModelSelector);
            _propertyToPredicate.Add(new Tuple<PropertyInfo, Expression<Func<TModel, TSearchModel, bool>>>(sortModelProperty, predicate));

            return this;
        }
        
        public IQueryable<TModel> ApplySearch(IQueryable<TModel> queryable, TSearchModel searchModel)
        {
            var searchModelPropertyInfos = searchModel.GetType().GetProperties().Where(p => p.GetType().IsValueType || p.GetValue(searchModel) != null);
            foreach (var searchModelPropertyInfo in searchModelPropertyInfos)
            {
                var predicates = FindPredicate(searchModelPropertyInfo);
                foreach (Expression<Func<TModel, TSearchModel, bool>> predicate in predicates)
                {
                    if (predicate != null)
                    {
                        Expression<Func<TModel, bool>> queryablePredicate = m => predicate.Invoke(m, searchModel);
                        queryable = queryable.Where(queryablePredicate.Expand());
                    }
                }
            }

            return queryable;
        }
        
        private IEnumerable<Expression<Func<TModel, TSearchModel, bool>>> FindPredicate(PropertyInfo searchPropertyInfo)
        {
            return _propertyToPredicate.Where(p => p.Item1 == searchPropertyInfo).Select(p => p.Item2);
        }
        
        private static PropertyInfo GetProperty<T, TValue>(Expression<Func<T, TValue>> selector)
        {
            var type = selector.Parameters[0].Type;

            Expression body = selector;
            if (body is LambdaExpression expression)
            {
                body = expression.Body;
            }

            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return (PropertyInfo)type.GetMember(((MemberExpression)body).Member.Name)[0];
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}