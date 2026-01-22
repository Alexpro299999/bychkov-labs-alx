using CandyShop.DataAccess.Models;
using CandyShop.DataAccess.Repositories;
using CandyShop.Web.Models;
using CandyShop.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CandyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public OrderVM OrderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Order> orderList;
            orderList = await _unitOfWork.Order.GetAllAsync(includeProperties: "ApplicationUser");
            return View(orderList);
        }

        public async Task<IActionResult> Details(int orderId)
        {
            OrderVM = new OrderVM()
            {
                Order = await _unitOfWork.Order.GetAsync(orderId, includeProperties: "ApplicationUser"),
                OrderDetail = await _unitOfWork.OrderDetail.GetAllAsync(includeProperties: "Candy")
            };
            // Фильтруем детали только для этого заказа вручную, так как GetFilteredAsync может быть удобнее,
            // но здесь используем GetAll и LINQ для простоты, или лучше использовать GetFilteredAsync если реализован.
            // В репозитории есть GetFilteredAsync, используем его для правильности.
            OrderVM.OrderDetail = await _unitOfWork.OrderDetail.GetFilteredAsync(u => u.OrderId == orderId, includeProperties: "Candy");

            return View(OrderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderDetail()
        {
            var orderHeaderFromDb = await _unitOfWork.Order.GetAsync(OrderVM.Order.Id);
            orderHeaderFromDb.Name = OrderVM.Order.Name;
            orderHeaderFromDb.PhoneNumber = OrderVM.Order.PhoneNumber;
            orderHeaderFromDb.StreetAddress = OrderVM.Order.StreetAddress;
            orderHeaderFromDb.City = OrderVM.Order.City;
            orderHeaderFromDb.State = OrderVM.Order.State;
            orderHeaderFromDb.PostalCode = OrderVM.Order.PostalCode;

            if (!string.IsNullOrEmpty(OrderVM.Order.Carrier))
            {
                orderHeaderFromDb.Carrier = OrderVM.Order.Carrier;
            }
            if (!string.IsNullOrEmpty(OrderVM.Order.TrackingNumber))
            {
                orderHeaderFromDb.TrackingNumber = OrderVM.Order.TrackingNumber;
            }

            await _unitOfWork.Order.UpdateAsync(orderHeaderFromDb);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Детали заказа обновлены";
            return RedirectToAction("Details", "Order", new { orderId = orderHeaderFromDb.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartProcessing()
        {
            var orderHeaderFromDb = await _unitOfWork.Order.GetAsync(OrderVM.Order.Id);
            orderHeaderFromDb.OrderStatus = "В обработке"; // Лучше использовать константы, но для простоты строка
            await _unitOfWork.Order.UpdateAsync(orderHeaderFromDb);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Статус заказа обновлен";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.Order.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShipOrder()
        {
            var orderHeaderFromDb = await _unitOfWork.Order.GetAsync(OrderVM.Order.Id);
            orderHeaderFromDb.TrackingNumber = OrderVM.Order.TrackingNumber;
            orderHeaderFromDb.Carrier = OrderVM.Order.Carrier;
            orderHeaderFromDb.OrderStatus = "Отправлен";
            orderHeaderFromDb.ShippingDate = DateTime.Now;

            await _unitOfWork.Order.UpdateAsync(orderHeaderFromDb);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Заказ отправлен";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.Order.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder()
        {
            var orderHeaderFromDb = await _unitOfWork.Order.GetAsync(OrderVM.Order.Id);
            if (orderHeaderFromDb.PaymentStatus == "Approved")
            {
                // Здесь логика возврата средств, если подключена платежная система
            }
            orderHeaderFromDb.OrderStatus = "Отменен";
            orderHeaderFromDb.PaymentStatus = "Возврат";

            await _unitOfWork.Order.UpdateAsync(orderHeaderFromDb);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Заказ отменен";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.Order.Id });
        }
    }
}