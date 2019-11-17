using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Common.Core;
using Ecommerce.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Ecommerce.Controllers
{
    public class ReviewsController : Controller
    {

        private IUnitOfWork _uof;


        public ReviewsController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        // GET: Reviews
        public ActionResult Index(int page)
        {
            try
            {
                var reviews = _uof.ProductReviewRepository.GetWithPagination(page);
                var pagedList = new StaticPagedList<ProductReview>(reviews.Items, reviews.PageNumber, reviews.PageSize, reviews.TotalItemCount);
                return View(pagedList);
            }
            catch (Exception)
            {
                return View();
            }
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var review = _uof.ProductReviewRepository.GetByID(id);
                return View(review);
            }
            catch (Exception )
            {
                return View();
            }
        }
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        public ActionResult Edit(int id)
        {
            var review = _uof.ProductReviewRepository.GetByID(id);
            return View(review);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                var review = _uof.ProductReviewRepository.GetByID(id);
                review.Comments = collection["comments"];
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        // GET: Reviews/Delete/5
        public ActionResult Delete(int id)
        {
            var review = _uof.ProductReviewRepository.GetByID(id);
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {

                _uof.ProductReviewRepository.Delete(id);
                _uof.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}