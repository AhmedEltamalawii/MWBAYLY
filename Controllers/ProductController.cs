using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MWBAYLY.Data;
using MWBAYLY.Helper_servec;
using MWBAYLY.Models;
using MWBAYLY.Repository.IRepository;
using System.Linq;
using System.Text;

namespace MWBAYLY.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly IRepository<Category> _categoryRepo;
        public ProductController(IProductRepository productRepo, IRepository<Category> categoryRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }
        public IActionResult Index(int page = 1, string? search = null) // 1, 2, 3
        {
            int pageSize = 5;
            if (page < 1)
                page = 1;

             var products = _productRepo.GetAll(c=>c.Category);

            if (search != null)
            {
                search = search.Trim();
                products = products.Where(e => e.Name.Contains(search) || e.Description.Contains(search) || e.Category.Name.Contains(search));
            }
            int totalProducts = products.Count();
            int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            products = products.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Search = search;


            if (products.Any())
            {
                return View(products.ToList());
            }
         

            return RedirectToAction("NotFoundPage", "Home");
        }


        public IActionResult CreateNew()
        {
            var Categories = _categoryRepo.GetAll().ToList();//.Select(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() });
                                                             // ViewBag.categories = Categories;
            ViewData["Categories"]=Categories;

            Product product = new Product();
            return View(product);
        }



        [HttpPost]
        public IActionResult CreateNew(Product product, IFormFile ImgUrl) // "1.jpg"
        {
            //product.ImgUrl = ImgUrl.FileName;
            //ModelState.Remove("ImgUrl");
            //Replace it validateNever in the Model Product 

            if (ModelState.IsValid)
            {
               

                product.ImgUrl = Imgfile.SaveImage(ImgUrl);
                _productRepo.CreateNew(product);
                _productRepo.Commit();

                TempData["success"] = "Add product successfully";
                return RedirectToAction(nameof(Index));
            }
            var Categories = _categoryRepo.GetAll().ToList();
            ViewData["Categories"] = Categories;
            return View(product);


        }


		public IActionResult Edit(int ProductId) // when you like make edit with use URl you must use Qeury String  Such AS "/Category/Edit?categoryId=3"
        {
            /*var product = context.Products.Where(e=>e.Id == ProductId)
                .Select(e => new Category()
                 {
                     Id = e.Id,
                     Name = e.Name,
                 }).FirstOrDefault();
             or */
            var Product = _productRepo.GetOne(p => p.Id == ProductId);

            var categories = _categoryRepo.GetAll().ToList();
            ViewData["allCategories"] = categories;
            //ViewBag.allCategories = categories;
         
            if (Product != null)

                return View(Product);

            return RedirectToAction(" NotFoundAction", "Home");
        }


        
        [HttpPost]
        public IActionResult Edit(Product product, IFormFile ImgUrl)
        {
            var oldProduct = _productRepo.GetOne(p => p.Id == product.Id);

            if (oldProduct == null)
                return RedirectToAction("NotFoundPage", "Home");

            product.ImgUrl = Imgfile.SaveImage(ImgUrl, oldProduct.ImgUrl);
            _productRepo.Edit(product);
            _productRepo.Commit();

            TempData["success"] = "Update Product successfully";
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int productId)
        {
            var oldProduct = _productRepo.GetOne(p => p.Id == productId);

            if (oldProduct == null) { return RedirectToAction("NotFoundAction", "Home"); }

            Imgfile.DeleteImage(oldProduct.ImgUrl);

            _productRepo.delete(oldProduct);
            _productRepo.Commit();

            TempData["success"] = "Delete product successfully";


            return RedirectToAction(nameof(Index));

     }
}
}
