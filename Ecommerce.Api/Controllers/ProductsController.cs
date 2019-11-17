using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Common.Core;
using Ecommerce.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IUnitOfWork _uof;

        public ProductsController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        // GET: api/Products
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            var products = _uof.ProductRepository.Get(x => x.ProductReview.Count > 0);
            return products;
        }
        [HttpGet("GetWithPagination/{page?}")]
        public StaticPagedList<Product> GetWithPagination(int page)
        {
            var pagedProducts = _uof.ProductRepository.GetWithPagination(page);
            return pagedProducts;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public Product Get(int id)
        {
            var product = _uof.ProductRepository.GetByID(id);
            return product;
        }

        // POST: api/Products
        [HttpPost]
        public void Post([FromBody] Product product)
        {
            _uof.ProductRepository.Insert(product);
            _uof.Commit();
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Product product)
        {
            var oldProduct = _uof.ProductRepository.GetByID(id);
            _uof.ProductRepository.Insert(product);
            _uof.Commit();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _uof.ProductRepository.Delete(id);
            _uof.Commit();
        }
    }
}
