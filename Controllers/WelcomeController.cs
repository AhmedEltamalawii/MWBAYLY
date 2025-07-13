using Microsoft.AspNetCore.Mvc;

namespace MWBAYLY.Controllers
{
    public class WelcomeController : Controller
    {
        public IActionResult Index()
        {
            List<string  > list=new List<string>() {"Ahmed","Esalm","MOhamed","Malek"};
            return View(model:list);
        }
    }
}
  