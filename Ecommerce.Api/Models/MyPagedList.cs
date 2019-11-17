using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Ecommerce.Api.Models
{
    public class PaginationEntity<TEntity> where TEntity : class
    {
        public PaginationEntity()
        {
            Items = new List<TEntity>();
        }
        public IEnumerable<TEntity> Items { get; set; }
        public PagedListMetaData MetaData { get; set; }
    }
}
