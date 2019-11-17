using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Ecommerce.Web.Controllers
{
    public class ReviewsController : Controller
    {
        private IConfiguration _configuration;
        private string _baseUrl;

        public ReviewsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _baseUrl = configuration.GetValue<string>("ApiUrl");
        }

        // GET: Reviews
        public ActionResult Index(int id)
        {
            return View();
        }


        // GET: Reviews/Create
        public ActionResult Create(int id)
        {
            return View();
        }

        // POST: Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int productId , IFormCollection collection)
        {
            try
            {
                var review = new ProductReview()
                {
                    ProductId = productId,
                    Comments = collection["Comments"],
                    EmailAddress = collection["EmailAddress"],
                    ReviewerName = collection["ReviewerName"],
                    Rating = 1
                };
                using (var client = new HttpClient())
                {
                    string jsonString = JsonConvert.SerializeObject(review);
                    HttpResponseMessage response = client.PostAsync(_baseUrl + "reviews",
                        new StringContent(jsonString, Encoding.UTF8, "application/json")).Result;

                    var output =  response.Content.ReadAsStringAsync().Result;
                    return RedirectToAction("Index", "Products");
                }
            }
            catch
            {
                return View();
            }
        }



    }
}