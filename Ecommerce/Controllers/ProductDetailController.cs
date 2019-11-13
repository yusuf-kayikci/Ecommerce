using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class ProductDetailController : Controller
    {

        private IUnitOfWork _unitOfWork;


        public ProductDetailController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: ProductDetais
        public ActionResult Index(int productId)
        {
            var product = _unitOfWork.ProductRepository.GetByID(productId);

            return View(product);
        }

        // GET: ProductDetais/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductDetais/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductDetais/Create
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

        // GET: ProductDetais/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductDetais/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductDetais/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductDetais/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}