using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ecommerce.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using X.PagedList;

namespace Ecommerce.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;
        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _baseUrl = configuration.GetValue<string>("ApiUrl");
        }

        // GET: Products
        public ActionResult Index(int page)
        {
            StaticPagedList<Product> pagedProducts;
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(_baseUrl + "products/GetWithPagination/" + page).Result;
                var jsonString = response.Content.ReadAsStringAsync().Result;
                PaginationModel<Product> products = JsonConvert.DeserializeObject<PaginationModel<Product>>(jsonString);
                pagedProducts = new StaticPagedList<Product>(products.Items, products.PageNumber, products.PageSize, products.TotalItemCount);
            }

            return View(pagedProducts);
        }

        // GET: Products/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
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

        // GET: Products/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Products/Edit/5
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

        // GET: Products/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Products/Delete/5
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