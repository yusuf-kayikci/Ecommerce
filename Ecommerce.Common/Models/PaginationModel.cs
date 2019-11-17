using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Common.Models
{
    public class PaginationModel<TEntity> where TEntity : class
    {
        public IEnumerable<TEntity> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
    }
}
