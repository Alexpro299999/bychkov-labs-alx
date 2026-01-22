using CandyShop.DataAccess.Models;
using CandyShop.DataAccess.Repositories;
using CandyShop.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CandyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> objCategoryList = await _unitOfWork.Category.GetAllAsync();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Category.AddAsync(obj);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Категория успешно создана";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = await _unitOfWork.Category.GetAsync(id.Value);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Category.UpdateAsync(obj);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Категория успешно обновлена";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = await _unitOfWork.Category.GetAsync(id.Value);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            var obj = await _unitOfWork.Category.GetAsync(id.Value);
            if (obj == null)
            {
                return NotFound();
            }

            await _unitOfWork.Category.DeleteAsync(id.Value);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Категория успешно удалена";
            return RedirectToAction("Index");
        }
    }
}