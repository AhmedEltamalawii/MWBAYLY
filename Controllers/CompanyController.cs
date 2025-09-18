using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MWBAYLY.Data;
using MWBAYLY.Models;
using MWBAYLY.Repository;
using MWBAYLY.Repository.IRepository;

namespace MWBAYLY.Controllers
{
    public class CompanyController : Controller
    {
      //  ApplicationDbContext context = new ApplicationDbContext();
        private readonly  ICompanyRepository _companyRepository;
        public CompanyController(ICompanyRepository _companyRepository)
        {
            this._companyRepository = _companyRepository;  
        }
        public IActionResult Index()
        {
            var companies = _companyRepository.GetAll(e=>e.Products); // use repository
            return View(companies);
        }

        public IActionResult Create()
        {
            Company company = new Company();

            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                _companyRepository.CreateNew(company);
                _companyRepository.Commit();
                TempData["success"] = "Company added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }
        public IActionResult Edit(int companyId)
        {
            var company = _companyRepository.GetOne(c => c.Id == companyId);
            if (company == null)
            {
                RedirectToAction("NotFoundAction", "Home");
            }
            return View(company);
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                _companyRepository.Edit(company);
                _companyRepository.Commit();
                TempData["success"] = "Company updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }



        public IActionResult Delete(int companyId)
        {
            var company = _companyRepository.GetOne(c => c.Id == companyId);
            if (ModelState.IsValid)
            {
                _companyRepository.delete(company);
                _companyRepository.Commit();
                TempData["success"] = "Company deleted successfully!";
                return RedirectToAction(nameof(Index));

            }
            return View(company);
        }


    }
}
