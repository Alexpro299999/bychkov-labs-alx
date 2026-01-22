using CandyShop.Web.Models;
using CandyShop.Web.Utility;
using Microsoft.AspNetCore.Mvc;

namespace CandyShop.Web.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("SessionCart");
            CartViewModel cartVM = new CartViewModel
            {
                CartItems = cart ?? new List<CartItem>(),
                GrandTotal = 0
            };

            foreach (var item in cartVM.CartItems)
            {
                cartVM.GrandTotal += (item.Candy.Price * item.Count);
            }

            return View(cartVM);
        }

        public IActionResult Plus(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("SessionCart");
            var cartItem = cart.FirstOrDefault(c => c.Candy.Id == id);
            if (cartItem != null)
            {
                cartItem.Count += 1;
            }
            HttpContext.Session.SetObjectAsJson("SessionCart", cart);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("SessionCart");
            var cartItem = cart.FirstOrDefault(c => c.Candy.Id == id);
            if (cartItem != null)
            {
                if (cartItem.Count <= 1)
                {
                    cart.Remove(cartItem);
                }
                else
                {
                    cartItem.Count -= 1;
                }
            }
            HttpContext.Session.SetObjectAsJson("SessionCart", cart);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("SessionCart");
            var cartItem = cart.FirstOrDefault(c => c.Candy.Id == id);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
            }
            HttpContext.Session.SetObjectAsJson("SessionCart", cart);
            return RedirectToAction(nameof(Index));
        }
    }
}