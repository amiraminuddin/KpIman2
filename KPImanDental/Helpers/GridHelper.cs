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

            if (inputDto.SortableInput != null)
            {
                query = OrderByProperty(query, inputDto.SortableInput, inputDto.SortableMode);
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

        private static IQueryable<T> OrderByProperty<T>(IQueryable<T> source, string propertyName, string orderMode)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(param, propertyName);
            var lambda = Expression.Lambda(property, param);
            var orderBy = "";

            // Detect whether we are ordering in ascending or descending order
            if(orderMode == "asc")
            {
                orderBy = "OrderBy";
            }else
            {
                orderBy = "OrderByDescending";
            }

            var orderByMethod = typeof(Queryable).GetMethods()
                .First(m => m.Name == orderBy && m.GetParameters().Length == 2);

            var genericMethod = orderByMethod.MakeGenericMethod(typeof(T), property.Type);

            return (IQueryable<T>)genericMethod.Invoke(null, new object[] { source, lambda });
        }
    }
}
