using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MWBAYLY.Models;
using MWBAYLY.Repository;
using MWBAYLY.Repository.IRepository;
using Stripe.Checkout;

namespace MWBAYLY.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public CartController(ICartRepository _cartRepository, UserManager<ApplicationUser> userManager)
        {
            this._cartRepository = _cartRepository;
            this.userManager = userManager;

        }
        public IActionResult AddCart(int count , int ProductId)
        {
            
                var applicationUserId = userManager.GetUserId(User);

                // Check if item already in cart
                var existingCart = _cartRepository
                    .GetAll()
                    .FirstOrDefault(c => c.ProductId == ProductId && c.ApplicationUserId == applicationUserId);

                if (existingCart != null)
                {
                   existingCart.Count += count;
                    _cartRepository.Edit(existingCart);
                
                }
                else
                {
                    Cart cart = new Cart()
                    {
                        Count = count,
                        ProductId = ProductId,
                        ApplicationUserId = applicationUserId
                    };
                    _cartRepository.CreateNew(cart);
                }

                _cartRepository.Commit();

                TempData["success"] = "Product added to cart successfully!";
                return RedirectToAction("Index", "Home");
         }

        
        public IActionResult Index()
        {
            var applicationUserId = userManager.GetUserId(User);

            var ShoppingCarts = _cartRepository.GetAll(e=>e.Product).Where(e => e.ApplicationUserId == applicationUserId);
            ViewBag.TotalPrice = ShoppingCarts.Sum(e => e.Product.Price * e.Count);

            return View(ShoppingCarts.ToList());
        }

        public IActionResult Increment(int productId)
        {
            var ApplicationUserId= userManager.GetUserId(User);
            var product=_cartRepository.GetOne(expression: e=> e.ProductId == productId || e.ApplicationUserId==ApplicationUserId);
            if (product != null)
            {
                product.Count++;
                _cartRepository.Commit();
                return RedirectToAction("Index");
            }


            return RedirectToAction("NotFoundAction","Home");
        }
        public IActionResult Decrement(int productId)
        {
            var ApplicationUserId = userManager.GetUserId(User);
            var product = _cartRepository.GetOne(expression: e => e.ProductId == productId || e.ApplicationUserId == ApplicationUserId);
            if (product != null)
            {
                product.Count--;
                if (product.Count > 0)

                    _cartRepository.Commit();
                else
                    _cartRepository.delete(product);
                return RedirectToAction("Index");
            }


            return RedirectToAction("NotFoundAction", "Home");
        }
        public IActionResult Delete(int productId)
        {
            var ApplicationUserId = userManager.GetUserId(User);
            var product = _cartRepository.GetOne(expression: e => e.ProductId == productId || e.ApplicationUserId == ApplicationUserId);
            if (product != null)
            {
                    
                 _cartRepository.delete(product);
                _cartRepository.Commit();
                return RedirectToAction("Index");
            }


            return RedirectToAction("NotFoundAction", "Home");
        }

        public IActionResult Pay()
        {
            var applicationUserId = userManager.GetUserId(User);

            var shoppingCarts = _cartRepository
                .GetAll(e => e.Product)
                .Where(e => e.ApplicationUserId == applicationUserId)
                .ToList();

            if (!shoppingCarts.Any())
            {
                return BadRequest("Your cart is empty.");
            }

            // Calculate total order amount
            decimal totalAmount = shoppingCarts.Sum(item => item.Product.Price * item.Count);

            if (totalAmount > 1000000000000m)
            {
                return BadRequest("Your order exceeds the maximum allowed total for a single payment.");
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
            };

            foreach (var item in shoppingCarts)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                            Description = item.Product.Description
                        },
                        UnitAmount = (long)(item.Product.Price * 100m)
                    },
                    Quantity = item.Count
                });
            }

            var service = new SessionService();
            var session = service.Create(options);

            return Redirect(session.Url);
        }

      
    }

}

