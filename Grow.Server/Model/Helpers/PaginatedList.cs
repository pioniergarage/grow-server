using Grow.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Grow.Server.Model.Helpers
{
    public class PaginatedList<TEntity> : List<TEntity> where TEntity : BaseNamedEntity
    {
        public int EntityCount { get; set; }
        public int PageCount{ get; set; }

        public PaginatedList(IQueryable<TEntity> entities, PaginationOptions options)
        {
            IEnumerable<TEntity> adaptedQuery = entities;

            // Filter and Sort
            adaptedQuery = FilterQuery(options, adaptedQuery);
            adaptedQuery = SortQuery(options, adaptedQuery);

            // Save max counts
            EntityCount = adaptedQuery.Count();
            PageCount = 1 + (EntityCount / options.PageSize);

            // Paging
            adaptedQuery = PageQuery(options, adaptedQuery);

            AddRange(adaptedQuery);
        }

        private static IEnumerable<TEntity> PageQuery(PaginationOptions options, IEnumerable<TEntity> adaptedQuery)
        {
            return adaptedQuery
                            .Skip((options.PageIndex - 1) * options.PageSize)
                            .Take(options.PageSize);
        }

        private IEnumerable<TEntity> SortQuery(PaginationOptions options, IEnumerable<TEntity> adaptedQuery)
        {
            return options.SortAscending
                ? adaptedQuery
                    .OrderBy(GetPropertySelector(options.SortColumn))
                : adaptedQuery
                    .OrderBy(GetPropertySelector(options.SortColumn));
        }

        private static IEnumerable<TEntity> FilterQuery(PaginationOptions options, IEnumerable<TEntity> adaptedQuery)
        {
            // Filter IsActive
            if (options.OnlyIfActive)
            {
                adaptedQuery = adaptedQuery
                    .Where(e => e.IsActive);
            }

            // Filter column value
            if (!string.IsNullOrEmpty(options.FilterColumn))
            {
                var property = Array.Find(
                    typeof(TEntity).GetProperties(),
                    p => p.Name.Equals(options.FilterColumn, StringComparison.CurrentCultureIgnoreCase)
                );

                if (property != null)
                {
                    adaptedQuery = adaptedQuery
                        .Where(e =>
                        {
                            var entityValue = property.GetValue(e)?.ToString();
                            return (entityValue == null && options.FilterValue == null)
                                || entityValue.Equals(options.FilterValue, StringComparison.CurrentCultureIgnoreCase);
                        });
                }
            }

            return adaptedQuery;
        }

        private Func<TEntity,object> GetPropertySelector(string propertyName)
        {
            var propInfo = typeof(TEntity).GetProperty(propertyName);
            return x => propInfo.GetValue(x, null);
        }
    }

    public class PaginationOptions
    {
        public int PageIndex
        {
            get { return _pageIndex; }
            set
            {
                _pageIndex = value < 1
                    ? 1
                    : value;
            }
        }
        private int _pageIndex;

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value < 10
                    ? 10
                    : value > 100
                        ? 100
                        : value;
            }
        }
        private int _pageSize;

        public string SortColumn { get; set; }

        public bool SortAscending { get; set; }

        public bool OnlyIfActive { get; set; }

        public string FilterColumn { get; set; }

        public string FilterValue { get; set; }

        public PaginationOptions()
        {
            // default values
            PageIndex       = 1;
            PageSize        = 25;
            SortColumn      = nameof(BaseNamedEntity.Name);
            SortAscending   = true;
            OnlyIfActive    = true;
            FilterColumn    = string.Empty;
            FilterValue     = string.Empty;
        }
    }
}
