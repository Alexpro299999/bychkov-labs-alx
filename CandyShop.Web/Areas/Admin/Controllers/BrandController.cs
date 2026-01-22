using CandyShop.DataAccess.Models;
using CandyShop.DataAccess.Repositories;
using CandyShop.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CandyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BrandController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Brand> objBrandList = await _unitOfWork.Brand.GetAllAsync();
            return View(objBrandList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand obj)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Brand.AddAsync(obj);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Бренд успешно создан";
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
            var brandFromDb = await _unitOfWork.Brand.GetAsync(id.Value);

            if (brandFromDb == null)
            {
                return NotFound();
            }
            return View(brandFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Brand obj)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Brand.UpdateAsync(obj);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Бренд успешно обновлен";
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
            var brandFromDb = await _unitOfWork.Brand.GetAsync(id.Value);

            if (brandFromDb == null)
            {
                return NotFound();
            }
            return View(brandFromDb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            var obj = await _unitOfWork.Brand.GetAsync(id.Value);
            if (obj == null)
            {
                return NotFound();
            }

            await _unitOfWork.Brand.DeleteAsync(id.Value);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Бренд успешно удален";
            return RedirectToAction("Index");
        }
    }
}