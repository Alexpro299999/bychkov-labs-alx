using CandyShop.DataAccess.Models;
using CandyShop.DataAccess.Repositories;
using CandyShop.Web.Models;
using CandyShop.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CandyShop.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("SessionCart");
            if (cart == null || cart.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ApplicationUser applicationUser = await _unitOfWork.ApplicationUser.GetAsync(claim.Value);
            applicationUser = (await _unitOfWork.ApplicationUser.GetFilteredAsync(u => u.Id == claim.Value)).FirstOrDefault();

            Order order = new Order
            {
                ApplicationUserId = claim.Value,
                Name = applicationUser.Name,
                StreetAddress = applicationUser.StreetAddress,
                City = applicationUser.City,
                State = applicationUser.State,
                PostalCode = applicationUser.PostalCode,
                PhoneNumber = applicationUser.PhoneNumber
            };

            foreach (var item in cart)
            {
                order.OrderTotal += (item.Candy.Price * item.Count);
            }

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("SessionCart");
            if (cart == null || cart.Count == 0)
            {
                ModelState.AddModelError("", "Корзина пуста");
                return RedirectToAction("Index", "Cart");
            }

            order.OrderDate = DateTime.Now;
            order.OrderStatus = "Принят";
            order.PaymentStatus = "В ожидании";

            order.OrderTotal = 0;
            foreach (var item in cart)
            {
                order.OrderTotal += (item.Candy.Price * item.Count);
            }

            await _unitOfWork.Order.AddAsync(order);
            await _unitOfWork.SaveAsync();

            foreach (var item in cart)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    CandyId = item.Candy.Id,
                    Price = item.Candy.Price,
                    Count = item.Count
                };
                await _unitOfWork.OrderDetail.AddAsync(orderDetail);
            }
            await _unitOfWork.SaveAsync();

            HttpContext.Session.SetObjectAsJson("SessionCart", null);

            return RedirectToAction(nameof(OrderConfirmation), new { id = order.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }
    }
}