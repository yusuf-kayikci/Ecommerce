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
        public IEnumerable<ProductReview> Get()
        {
            var reviews = _uof.ProductReviewRepository.Get();
            return reviews;
        }

        [HttpGet("GetWithPagination/{page?}")]
        public StaticPagedList<ProductReview> GetWithPagination(int page)
        {
            var pagedReviews = _uof.ProductReviewRepository.GetWithPagination(page);
            return pagedReviews;
        }

        // GET: api/Reviews/5
        [HttpGet("{id}", Name = "Get")]
        public ProductReview Get(int id)
        {
            var review = _uof.ProductReviewRepository.GetByID(id);
            return review;
        }

        // POST: api/Reviews
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
        public void Delete(int id)
        {
            _uof.ProductReviewRepository.Delete(id);
            _uof.Commit();
        }
    }
}
