using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MWBAYLY.Data;
using MWBAYLY.Models;
using System.Linq;
using System.Text;

namespace MWBAYLY.Controllers
{
    public class ProductController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public IActionResult Index(int page)
        {
            
            if (page<=0)
            
                page = 1;
                var Products = context.Products.Include(e => e.Category).Skip((page - 1) * 5).Take(4).ToList();

            //if (Products.Count)
            //{ return View(Products);
            //}
            if (Products.Any())
            {
                return View(Products.ToList());
            }

            return RedirectToAction("NotFoundPage", "Home");

        }

        public IActionResult CreateNew()
        {
            var Categories = context.categories.ToList();//.Select(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() });
           // ViewBag.categories = categories;
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
                if (ImgUrl.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName); // ".jpg"
                                                                                                   // var fileName = ImgUrl.FileName; //  name of photo 1.pnj
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        ImgUrl.CopyTo(stream);
                    }

                    product.ImgUrl = fileName;
                }

                context.Products.Add(product);
                context.SaveChanges();
                TempData["success"] = "Add category successfully";
                return RedirectToAction(nameof(Index));
            }
            var Categories = context.categories.ToList();
            ViewData["Categories"] = Categories;
            return View(product);


        }

       


		//public IActionResult CreateNew(Product product ,IFormFile ImgUrl)
		//{
		//    //if (ImgUrl.Length > 0)
		//    //{
		//        var FileName = Path.GetTempFileName();
		//        var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot \\ images");
		//        using (var stream = System.IO.File.Create(FileName))
		//        {
		//            ImgUrl.CopyTo(stream);
		//        }
		//        product.ImgUrl = ImgUrl.FileName;
		//    //}
		//    context.Products.Add(product);
		//    context.SaveChanges();
		//    return RedirectToAction(nameof(Index));

		//}
		//[HttpPost]
		//public IActionResult CreateNew(String ProductName)

		//{
		//    Category category = new Category() { Name = ProductName };
		//    context.categories.Add(category);
		//    context.SaveChanges();
		//    return RedirectToAction(nameof(Index));

		//}


		public IActionResult Edit(int ProductId) // when you like make edit with use URl you must use Qeury String  Such AS "/Category/Edit?categoryId=3"
        {
            var Product = context.Products.Find(ProductId);
            var categories = context.categories.ToList();
            ViewData["allCategories"] = categories;
            //ViewBag.allCategories = categories;
         
            if (Product != null)

                return View(Product);

            return RedirectToAction(" NotFoundAction", "Home");
        }


        [HttpPost]
       

        public IActionResult Edit(Product product, IFormFile ImgUrl)
        {
            //Category category = new Category() { Name = CategoryName };
            // here without NoTracking the error apper because the efCore The data follow To Road of data but when i use the AsNotracking Repair this error 
            var oldProduct =context.Products.AsNoTracking().FirstOrDefault(e=>e.Id==product.Id);
            if (ImgUrl != null && ImgUrl.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName); // ".jpg"
                                                                                               //  var fileName = ImgUrl.FileName; //  name of photo 1.pnj
                var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\images", fileName);
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\images", fileName, oldProduct.ImgUrl);

                using (var stream = System.IO.File.Create(FilePath))
                {
                    ImgUrl.CopyTo(stream);
                }

                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
                product.ImgUrl = fileName;

            }
			else
			{
				product.ImgUrl = oldProduct.ImgUrl;
			}

			context.Products.Update(product);
			context.SaveChanges();

			TempData["success"] = "Update category successfully";


			return RedirectToAction(nameof(Index));
		}

        //public IActionResult Edit(Product product, IFormFile ImgUrl) // "1.jpg"
        //{
        //    //var oldProduct = context.Products.AsNoTracking().FirstOrDefault(e => e.Id == product.Id);
        //    if ( ImgUrl.Length > 0)
        //    {
        //        //var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName); // ".jpg"
        //        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
        //    //    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", oldProduct.ImgUrl);

        //        using (var stream = System.IO.File.Create(filePath))
        //        {
        //            ImgUrl.CopyTo(stream);
        //        }




        //        product.ImgUrl = fileName;
        //    }
        //    else
        //    {
        //        product.ImgUrl = oldProduct.ImgUrl;
        //    }

        //    context.Products.Update(product);
        //    context.SaveChanges();

        //    TempData["success"] = "Update category successfully";


        //    return RedirectToAction(nameof(Index));
        //}



        //public IActionResult Delete(int productId)
        //{
        //    var oldProduct = context.Products.AsNoTracking().FirstOrDefault(e => e.Id == productId);
        //    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\images", oldProduct.ImgUrl);

        //    if (System.IO.File.Exists(oldFilePath))
        //    {
        //        System.IO.File.Delete(oldFilePath);
        //    }

        //    Product product = new Product() { Id = productId };
        //    context.Products.Remove(product);
        //    context.SaveChanges();

        //    TempData["success"] = "Delete product successfully";


        //    return RedirectToAction(nameof(Index));
        //}



        public IActionResult Delete(int productId)
        {
            var oldProduct = context.Products.AsNoTracking().FirstOrDefault(e => e.Id == productId);

            if (oldProduct != null) { RedirectToAction("NotFoundAction", "Home"); }

            if(!string.IsNullOrEmpty(oldProduct.ImgUrl)) 
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\images", oldProduct.ImgUrl);

                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

            }


            Product product = new Product() { Id = productId };
            context.Products.Remove(product);
            context.SaveChanges();

            TempData["success"] = "Delete product successfully";


            return RedirectToAction(nameof(Index));

     }   }
}
