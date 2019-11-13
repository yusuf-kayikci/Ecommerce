using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ecommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NonFactors.Mvc.Grid;
using Omu.AwesomeMvc;

namespace Ecommerce.Controllers
{
    public class ProductController : Controller
    {
        
        private IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            return View(null);
        }

        // GET: Product
        [HttpPost]
        public ActionResult GetItems(GridParams g)
        {

            Expression<Func<Product, object>>[] includes = { (x => x.ProductProductPhoto) };

            var products = _unitOfWork.ProductRepository.IncludeMultiple(includes , x => x.ProductProductPhoto.Count >  0).AsQueryable();

            //Expression<Func<Product, object>>[] includeFunction = { (x => x.ProductModel) , (x => x.ProductReview)};
            //Expression<Func<Product, bool>> filter = (x => x.ProductModelId != null && x.ProductReview.Count > 0); 
            //var products = _unitOfWork.ProductRepository.IncludeMultiple(includeFunction ,filter).ToList();

            //return View(products);


            return Json(new GridModelBuilder<Product>(products, g)
            {
                Key = "Id" ,// needed for Entity Framework | nesting | tree | api
                KeyProp = p => p.ProductId,
                Map = p => new
                {
                    p.ProductId,
                    p.Name,
                    p.ModifiedDate,
                    p.ListPrice,
                    p.SellStartDate,
                }
            }.Build());
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
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

                return RedirectToAction(nameof(GetItems));
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(GetItems));
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Product/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            { 
                _unitOfWork.ProductRepository.Delete(id);
                _unitOfWork.Commit();                
                return RedirectToAction(nameof(GetItems));
            }
            catch
            {
                return View();
            }
        }
    }
}