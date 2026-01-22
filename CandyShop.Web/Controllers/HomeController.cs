using CandyShop.DataAccess.Repositories;
using CandyShop.Web.Models;
using CandyShop.Web.Utility;
using CandyShop.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CandyShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["CurrentFilter"] = searchString;

            var candies = await _unitOfWork.Candy.GetAllAsync(includeProperties: "Category,Brand");

            if (!String.IsNullOrEmpty(searchString))
            {
                candies = candies.Where(s => s.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                                       || s.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    candies = candies.OrderByDescending(s => s.Name);
                    break;
                case "Price":
                    candies = candies.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    candies = candies.OrderByDescending(s => s.Price);
                    break;
                default:
                    candies = candies.OrderBy(s => s.Name);
                    break;
            }

            return View(candies);
        }

        public async Task<IActionResult> Details(int productId)
        {
            CartItem cartObj = new CartItem()
            {
                Count = 1,
                Candy = await _unitOfWork.Candy.GetAsync(productId, includeProperties: "Category,Brand")
            };
            return View(cartObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(CartItem shoppingCart)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("SessionCart");
            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            var candyFromDb = await _unitOfWork.Candy.GetAsync(shoppingCart.Candy.Id, includeProperties: "Category,Brand");

            if (candyFromDb == null)
            {
                return NotFound();
            }

            var cartItemInCart = cart.FirstOrDefault(c => c.Candy.Id == shoppingCart.Candy.Id);
            if (cartItemInCart != null)
            {
                cartItemInCart.Count += shoppingCart.Count;
            }
            else
            {
                cart.Add(new CartItem { Candy = candyFromDb, Count = shoppingCart.Count });
            }

            HttpContext.Session.SetObjectAsJson("SessionCart", cart);

            TempData["success"] = "Товар добавлен в корзину";
            return RedirectToAction(nameof(Index));
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