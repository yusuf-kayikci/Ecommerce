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
    public class ReviewsController : ControllerBase
    {
        private IUnitOfWork _uof;

        public ReviewsController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        // GET: api/Reviews
        [HttpGet]
        public ActionResult<IEnumerable<ProductReview>> Get()
        {
            try
            {
                var reviews = _uof.ProductReviewRepository.Get();
                return Ok(reviews);
            }
            catch
            {
                return BadRequest();
            }

        }

        //[HttpGet("GetWithPagination/{page?}")]
        //public StaticPagedList<ProductReview> GetWithPagination(int page)
        //{
        //    var pagedReviews = _uof.ProductReviewRepository.GetWithPagination(page);
        //    return pagedReviews;
        //}

        // GET: api/Reviews/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<ProductReview> Get(int id)
        {
            try
            {
                var review = _uof.ProductReviewRepository.GetByID(id);
                return Ok(review);
            }
            catch
            {
                return BadRequest();
            }


        }

        // POST: api/Reviews
        [HttpPost]
        public ActionResult<ProductReview> Post([FromBody]ProductReview review)
        {
            try
            {
                _uof.ProductReviewRepository.Insert(review);
                _uof.Commit();
                return Ok(review);
            }
            catch (Exception)
            {
                return BadRequest();
            }


        }

        // PUT: api/Reviews/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] ProductReview value)
        {
            _uof.ProductReviewRepository.Insert(value);
            _uof.Commit();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _uof.ProductReviewRepository.Delete(id);
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
