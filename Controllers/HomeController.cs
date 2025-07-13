using Microsoft.AspNetCore.Mvc;
using MWBAYLY.Data;
using MWBAYLY.Models;
using System.Diagnostics;

namespace MWBAYLY.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext Context = new ApplicationDbContext();

		private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        { 
            var product = Context.Products.ToList();
            return View(model: product);
        }


        public IActionResult Details(int id)
        {
            var product = Context.Products.Find(id);
            if (product != null)
            {
                return View(product);
            
            }
            return RedirectToAction(nameof(NotFoundAction));

        }

        public IActionResult NotFoundAction()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
