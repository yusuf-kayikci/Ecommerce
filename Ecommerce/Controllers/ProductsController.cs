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
            Expression<Func<Product, object>>[] includeFunction = { (x => x.ProductModel), (x => x.ProductReview) };
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
        public ActionResult Create(int id, IFormCollection collection)
        {
            try
            {
                var product = new Product()
                {
                    Name = collection["Name"],
                    ProductNumber = "AA-" + Guid.NewGuid().ToString().Substring(0, 3),
                    SafetyStockLevel = Convert.ToInt16(collection["SafetyStockLevel"]),
                    Color = collection["Color"],
                    StandardCost = Convert.ToDecimal(collection["StandardCost"]),
                    ListPrice = Convert.ToDecimal(collection["ListPrice"]),
                    DaysToManufacture = Convert.ToInt32(collection["DaysToManufacture"]),
                    SellStartDate = Convert.ToDateTime(collection["SellStartDate"]),
                    SellEndDate = Convert.ToDateTime(collection["SellEndDate"]),
                    ModifiedDate = DateTime.Now,
                    ReorderPoint = 1

                };
                _uof.ProductRepository.Insert(product);
                _uof.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _uof.Rollback();
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            var product = _uof.ProductRepository.GetByID(id);
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                var product = _uof.ProductRepository.GetByID(id);

                product.Name = collection["Name"];
                //ProductNumber = Guid.NewGuid().ToString().Substring(0, 3),  just for create
                product.SafetyStockLevel = Convert.ToInt16(collection["SafetyStockLevel"]);
                product.Color = collection["Color"];
                product.StandardCost = Convert.ToDecimal(collection["StandardCost"]);
                product.ListPrice = Convert.ToDecimal(collection["ListPrice"]);
                product.DaysToManufacture = Convert.ToInt32(collection["DaysToManufacture"]);
                product.SellStartDate = Convert.ToDateTime(collection["SellStartDate"]);
                product.SellEndDate = Convert.ToDateTime(collection["SellEndDate"]);
                product.ModifiedDate = DateTime.Now;
                product.ReorderPoint = 1;

                _uof.ProductRepository.Update(product);
                _uof.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
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
                //CASCADE PROBLEM WİTH MULTI TABLE NOT JUST REVIEWS
                //var reviewsOfProduct = _uof.ProductReviewRepository.Get(x => x.ProductId == id);
                //foreach (var review in reviewsOfProduct)
                //{
                //    _uof.ProductReviewRepository.Delete(review.ProductReviewId);
                //}
                var product = _uof.ProductRepository.GetByID(id);
                _uof.ProductRepository.Delete(product);
                _uof.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _uof.Rollback();
                return View();
            }
        }
    }
}