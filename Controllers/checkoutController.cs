using Microsoft.AspNetCore.Mvc;

namespace MWBAYLY.Controllers
{
    public class checkoutController : Controller
    {
        public IActionResult success()
        {
            TempData["success"] = "✅ Payment completed successfully! Thank you for your order.";
            return View();
        }
        public IActionResult error()
        {
            
            return View();
        }
    }
}
