using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ecommerce.Common.Core;
using Ecommerce.Common.Models;
using Microsoft.AspNetCore.Mvc;

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
            var products = _uof.ProductRepository.Get();
            return products;
        }
        [HttpGet("GetWithPagination/{page?}")]
        public ActionResult<PaginationModel<Product>>  GetWithPagination(int page)
        {
            try
            {
                var pagedProducts = _uof.ProductRepository.GetWithPagination(page, 10);
                return Ok(pagedProducts);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {
            try
            {
                Expression<Func<Product, object>>[] includes = { (x => x.ProductReview) };
                var product = _uof.ProductRepository.IncludeMultiple(includes, x => x.ProductId == id).SingleOrDefault();
                return Ok(product);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        // POST: api/Products
        [HttpPost]
        public ActionResult Post([FromBody] Product product)
        {
            try
            {
                _uof.ProductRepository.Insert(product);
                _uof.Commit();
                return Ok();
            }
            catch (Exception)
            {
                _uof.Rollback();
                return BadRequest();
            }


        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Product product)
        {
            try
            {
                _uof.ProductRepository.Update(product);
                _uof.Commit();
                return Ok();
            }
            catch (Exception)
            {
                _uof.Rollback();
                return BadRequest();
            }

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _uof.ProductRepository.Delete(id);
                _uof.Commit();
                return Ok();
            }
            catch (Exception)
            {
                _uof.Rollback();
                return BadRequest();
            }


        }


    }
}
