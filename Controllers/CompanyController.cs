using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MWBAYLY.Data;
using MWBAYLY.Models;
using MWBAYLY.Repository;

namespace MWBAYLY.Controllers
{
    public class CompanyController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public IActionResult Index()
        {
            var Campanies = context.Campanies.Include(e => e.Products).ToList();
            return View(Campanies);
        }

        public IActionResult CreateNew()
        {
            Company company = new Company();

            return View(company);
        }

        [HttpPost]
        public IActionResult CreateNew(Company company)
        {
            if (ModelState.IsValid)
            {
                // Company company = new Company() { Name = CompanyName };

                //this Code related with Cookies
                //CookieOptions options = new CookieOptions();
                //  options.Secure= true;
                //  options.Expires = DateTimeOffset.Now.AddDays(2);
                //  Response.Cookies.Append("success", " Operation Success ", options);
                //context.categories.Add(company);
                //context.SaveChanges();
                //Soild
                context.Campanies.Add(company);
                TempData["success"] = "Add Successfuly ";
                context.SaveChanges();
                return RedirectToAction(nameof(Index));

            }
            return View(company);
        }
    }
}
