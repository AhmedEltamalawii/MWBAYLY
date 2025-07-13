using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MWBAYLY.Data;
using MWBAYLY.Models;
using MWBAYLY.Repository;
using MWBAYLY.Repository.IRepository;

namespace MWBAYLY.Controllers
{
    public class CategoryController : Controller
    {
        //ApplicationDbContext context = new ApplicationDbContext();//First With I Learn Before Learing SOLD
        // ICategoryRepository-->this Is Location In heep Time  categoryRepository=new CategoryRepository--> This is location RunTime();
        private readonly ICategoryRepository categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            //  var categories =  context.categories.Include(e=>e.Products).ToList(); 
            var categories=categoryRepository.GetAll(e=>e.Products);// OR var categories=categoryRepository.GetAll("Products", e=>e.Name.Contains("AWE" ---> this is Experssion));
          //  var categories = categoryRepository.GetAll([e => e.Products,e=>e.Name]);// If you use many include go to put [] arry in IRepository
            //ViewBag.success = TempData["success"];
            //ViewBag.success = Request.Cookies["success"];

            return View(model: categories); 
         
        }
        [HttpGet("Category/Create")]
        public IActionResult CreateNew()
        {
            Category category = new Category();

            return View(category) ;
        }
        
        [HttpPost]
        public IActionResult CreateNew(Category category)
        {
            if (ModelState.IsValid)
            {
                // Category category = new Category() { Name = CategoryName };

                //this Code related with Cookies
                //CookieOptions options = new CookieOptions();
                //  options.Secure= true;
                //  options.Expires = DateTimeOffset.Now.AddDays(2);
                //  Response.Cookies.Append("success", " Operation Success ", options);
                //context.categories.Add(category);
                //context.SaveChanges();
                //Soild
                categoryRepository.CreateNew(category);
                categoryRepository.Commit();
                 TempData["success"] = "Add Successfuly ";
                return RedirectToAction(nameof(Index));
                    
            }

         return View(category);
        }

    

        public IActionResult Edit(int categoryId) // when you like make edit with use URl you must use Qeury String  Such AS "/Category/Edit?categoryId=3"
        {
            //  var category = context.categories.Find(categoryId);

            var category = categoryRepository.GetOne(e => e.Id == categoryId);

               if (category != null)

                   return View(category);

                return RedirectToAction(" NotFoundAction", "Home");
           
           
        }


        [HttpPost]
        public IActionResult Edit(Category category)
        {
            //Category category = new Category() { Name = CategoryName };
          if(ModelState.IsValid) 
            {
                //context.categories.Update(category);
                //context.SaveChanges();
                categoryRepository.Edit(category);
                categoryRepository.Commit();
                TempData["success"] = " Update Successfuly ";
                return RedirectToAction(nameof(Index));
            }
          return View(category);
            

        }


        public IActionResult Delete(int CategoryId)
        {
            Category category = new Category() {Id = CategoryId };
            //context.categories.Remove(category);
            //context.SaveChanges();
            categoryRepository.delete(category);   
            categoryRepository.Commit();
            
            TempData["success"] = "Delete Successfuly ";
            return RedirectToAction(nameof(Index));

        }














        //public IActionResult Edit(Category Category)
        //{
        //    context.categories.Find(id); 
        //    context.SaveChanges();
        //    return RedirectToAction(nameof(Index));2

        //}
        //public IActionResult Edit(int  )
        //{
        //    context.categories.Update(ategoryId);
        //    context.SaveChanges();
        //    return RedirectToAction(nameof(Index));

        //  }

    }
}
