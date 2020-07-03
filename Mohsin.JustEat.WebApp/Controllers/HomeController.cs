using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mohsin.JustEat.Services;
using Mohsin.JustEat.WebApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mohsin.JustEat.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IRestaurantFinder _restaurantFinder;

        public HomeController(ILogger<HomeController> logger,IRestaurantFinder restaurantFinder)
        {
            _logger = logger;
            _restaurantFinder = restaurantFinder;
        }

        public IActionResult Index(string searchString)
        {
            //var text = postCode
            return View();
        }

        public async Task<ActionResult> Restaurants(string searchString)
        {
            try
            {
                var result = await _restaurantFinder.FindByPostCodeAsync(searchString);
                var json = JsonConvert.SerializeObject(result);
                ViewData["json"] = JToken.Parse(json).ToString(Formatting.Indented);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { 
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, 
                    Message = ex.Message});
            }
            return View();

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
