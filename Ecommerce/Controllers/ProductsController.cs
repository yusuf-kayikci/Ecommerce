using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ecommerce.Common.Core;
using Ecommerce.Common.Models;
using Ecommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Ecommerce.Controllers
{
    public class ProductsController : Controller
    {
        private IUnitOfWork _uof;

        public ProductsController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        // GET: Product
        public ActionResult Index(int page)
        {
            var products = _uof.ProductRepository.GetWithPagination(page);
            var pagedProducts = new StaticPagedList<Product>(products.Items, products.PageNumber, products.PageSize, products.TotalItemCount);
            return View(pagedProducts);
        }


        // GET: v/Details/5
        public ActionResult Details(int id)
        {
            Expression<Func<Product, object>>[] includeFunction = { (x => x.ProductModel) , (x => x.ProductReview)};
            var product = _uof.ProductRepository.IncludeMultiple(includeFunction, (x => x.ProductId == id)).SingleOrDefault();            
            return View(product);
        }

        // GET: Product/Create
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

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            Expression<Func<Product, object>>[] includes = { (x => x.ProductModel) , (x => x.ProductReview) , (x => x.ProductSubcategory)};
            var product = _uof.ProductRepository.IncludeMultiple(includes , (x => x.ProductId == id)).SingleOrDefault();
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product product)
        {
            try
            {
                _uof.ProductRepository.Update(product);
                _uof.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            var product = _uof.ProductRepository.GetByID(id);
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var product = _uof.ProductRepository.GetByID(id);
                _uof.ProductRepository.Delete(product);
                _uof.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }
    }
}