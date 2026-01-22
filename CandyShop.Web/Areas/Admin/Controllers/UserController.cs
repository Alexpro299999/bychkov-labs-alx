using CandyShop.DataAccess.Models;
using CandyShop.DataAccess.Repositories;
using CandyShop.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CandyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var userList = await _unitOfWork.ApplicationUser.GetAllAsync();
            return View(userList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockUnlock(string id)
        {

            var allUsers = await _unitOfWork.ApplicationUser.GetAllAsync();
            var user = allUsers.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                // Пользователь заблокирован -> разблокируем
                user.LockoutEnd = DateTime.Now;
            }
            else
            {
                // Пользователь активен -> блокируем на 1000 лет
                user.LockoutEnd = DateTime.Now.AddYears(1000);
            }

            await _unitOfWork.ApplicationUser.UpdateAsync(user);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}