using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Common.Core;
using Ecommerce.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var reviews = _uof.ProductReviewRepository.GetWithPagination(page);
            return View(reviews);
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int id)
        {
            var review = _uof.ProductReviewRepository.GetByID(id);
            return View(review);
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
                // TODO: Add insert logic here

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