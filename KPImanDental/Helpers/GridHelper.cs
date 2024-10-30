using AutoMapper;
using KPImanDental.Dto.GridDto;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace KPImanDental.Helpers
{
    public static class GridHelper
    {
        public static async Task<GridDto<IEnumerable<TDto>>> GetGridDataAsync<TEntity, TDto>(
            IQueryable<TEntity> queryable,
            IMapper mapper,
            GridInputDto inputDto
            ) where TEntity : class
        {
            var query = queryable;

            if (inputDto.SortMeta != null)
            {
                query = OrderByProperty(query, inputDto.SortMeta);
            }

            if (!string.IsNullOrEmpty(inputDto.FilterValue)) 
            {
                query = FilterByProperty(query, inputDto.FilterColumn, inputDto.FilterValue);
            }

            if (inputDto.WhereCondition.Count != 0)
            {
                query = WhereByProperty(query, inputDto.WhereCondition);
            }

            var totalCount = await query.CountAsync();
            query = query
                .Skip(((inputDto.CurrentPage - 1) * inputDto.PageSize))
                .Take(inputDto.PageSize)
                .AsQueryable();

            var entityList = await query.ToListAsync();
            var entityListDto = mapper.Map<IEnumerable<TDto>>(entityList);

            return new GridDto<IEnumerable<TDto>>
            {
                Data = entityListDto,
                TotalData = totalCount,
                PageSize = inputDto.PageSize
            };
        }
        private static IQueryable<T> OrderByProperty<T>(IQueryable<T> source, List<SortMeta> sortMeta)
        {
            IOrderedQueryable<T> orderedQuery = null; // To hold the ordered query
            var param = Expression.Parameter(typeof(T), "x"); // Parameter for the lambda expression

            foreach (var sort in sortMeta)
            {
                var property = Expression.Property(param, sort.Field); // Get property expression
                var lambda = Expression.Lambda(property, param); // Create lambda expression

                // Detect whether we are ordering in ascending or descending order
                var orderByMethod = typeof(Queryable).GetMethods()
                    .First(m => m.Name == (sort.Order == 1 ? "OrderBy" : "OrderByDescending") && m.GetParameters().Length == 2);

                var genericMethod = orderByMethod.MakeGenericMethod(typeof(T), property.Type);

                // Apply the ordering
                if (orderedQuery == null)
                {
                    orderedQuery = (IOrderedQueryable<T>)genericMethod.Invoke(null, new object[] { source, lambda });
                }
                else
                {
                    // Use ThenBy or ThenByDescending for subsequent sorting
                    orderByMethod = typeof(Queryable).GetMethods()
                        .First(m => m.Name == (sort.Order == 1 ? "ThenBy" : "ThenByDescending") && m.GetParameters().Length == 2);

                    genericMethod = orderByMethod.MakeGenericMethod(typeof(T), property.Type);

                    orderedQuery = (IOrderedQueryable<T>)genericMethod.Invoke(null, new object[] { orderedQuery, lambda });
                }
            }

            return orderedQuery ?? source;

        }

        private static IQueryable<T> FilterByProperty<T>(IQueryable<T> source, List<FilterColumn>  propertiesName, object value)
        {
            // Parameter expression for the lambda (e.g., x => ...)
            var parameter = Expression.Parameter(typeof(T), "x");

            // Create a list to store individual property equality expressions
            var expressions = new List<Expression>();

            foreach (var propertyName in propertiesName)
            {
                // Use propertyName.Field to access the actual field name
                var property = typeof(T).GetProperty(propertyName.Field);

                if (property == null)
                    throw new ArgumentException($"Property '{propertyName.Field}' does not exist on type '{typeof(T)}'.");

                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                Expression expression;

                if(property.PropertyType == typeof(string))
                {
                    var constant = Expression.Constant(value.ToString(), typeof(string));
                    expression = Expression.Call(propertyAccess, "Contains", null, constant);
                    expressions.Add(expression);
                }
                else if(property.PropertyType == typeof(int) || property.PropertyType == typeof(double) || property.PropertyType == typeof(float) || property.PropertyType == typeof(decimal))
                {
                    if(double.TryParse(value.ToString(), out double parsedValue))
                    {
                        var constant = Expression.Constant(Convert.ChangeType(parsedValue, property.PropertyType));
                        expression = Expression.Equal(propertyAccess, constant);
                        expressions.Add(expression);
                    }
                }else
                {
                    var constant = Expression.Constant(Convert.ChangeType(value, property.PropertyType));
                    expression = Expression.Equal(propertyAccess, constant);
                    expressions.Add(expression);
                }                
            }

            // Combine all expressions using 'OrElse' to create a composite OR expression
            var combinedExpression = expressions.Aggregate(Expression.OrElse);

            // Build the final lambda expression (x => x.Property1 == value || x.Property2 == value ...)
            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);

            // Apply the filter
            return source.Where(lambda);
        }

        private static IQueryable<T> WhereByProperty<T>(IQueryable<T> source, List<WhereClause> whereClauses)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var expressions = new List<Expression>(); ;

            foreach (var whereClause in whereClauses) 
            {
                var property = typeof(T).GetProperty(whereClause.Field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                Expression expression;

                if(property.PropertyType == typeof(string))
                {
                    var constant = Expression.Constant(whereClause.Value.ToString(), (property.PropertyType));
                    expression = Expression.Equal(propertyAccess, constant);
                    expressions.Add(expression);
                }
                else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(double) || property.PropertyType == typeof(float) || property.PropertyType == typeof(decimal) || property.PropertyType == typeof(long) || property.PropertyType == typeof(bool))
                {
                    var constant = Expression.Constant(Convert.ChangeType(whereClause.Value,property.PropertyType));
                    expression = Expression.Equal(propertyAccess, constant);
                    expressions.Add(expression);
                }
            }

            var combinedExpression = expressions.Aggregate(Expression.And);

            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
            // Apply the filter
            return source.Where(lambda);
        }
    }
}
