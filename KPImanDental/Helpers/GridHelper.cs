using AutoMapper;
using KPImanDental.Dto.GridDto;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
            var totalCount = await queryable.CountAsync();
            var query = queryable;

            if (inputDto.SortMeta != null)
            {
                query = OrderByProperty(query, inputDto.SortMeta);
            }

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

        //private static IQueryable<T> OrderByProperty<T>(IQueryable<T> source, List<SortMeta> sortMeta)
        //{
        //    var param = Expression.Parameter(typeof(T), "x");
        //    var property = Expression.Property(param, sortMeta.field);
        //    var lambda = Expression.Lambda(property, param);
        //    var orderBy = "";

        //    // Detect whether we are ordering in ascending or descending order
        //    if (orderMode == "asc")
        //    {
        //        orderBy = "OrderBy";
        //    }
        //    else
        //    {
        //        orderBy = "OrderByDescending";
        //    }

        //    var orderByMethod = typeof(Queryable).GetMethods()
        //        .First(m => m.Name == orderBy && m.GetParameters().Length == 2);

        //    var genericMethod = orderByMethod.MakeGenericMethod(typeof(T), property.Type);

        //    return (IQueryable<T>)genericMethod.Invoke(null, new object[] { source, lambda });
        //}

        private static IQueryable<T> OrderByProperty<T>(IQueryable<T> source, List<SortMeta> sortMeta)
        {
            if (sortMeta == null || !sortMeta.Any())
            {
                return source; // Return the original source if there are no sorting criteria
            }

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
    }
}
