using Microsoft.AspNetCore.Mvc;
using MWBAYLY.Data;
using MWBAYLY.Models;
using MWBAYLY.Utlity;
using System.Diagnostics;

namespace MWBAYLY.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext Context = new ApplicationDbContext();

		private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }
        private readonly IEmailSender _emailSender;

      

        [HttpGet]
        public IActionResult email()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> email(string email, string subject, string message)
        {
            await _emailSender.SendEmailAsync(email, subject, message);
            ViewBag.Status = "تم إرسال الإيميل بنجاح!";
            return View();
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
