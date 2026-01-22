using CandyShop.DataAccess.Models;
using CandyShop.DataAccess.Repositories;
using CandyShop.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CandyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CandyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CandyController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Candy> objCandyList = await _unitOfWork.Candy.GetAllAsync(includeProperties: "Category,Brand");
            return View(objCandyList);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.CategoryList = (await _unitOfWork.Category.GetAllAsync()).Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            ViewBag.BrandList = (await _unitOfWork.Brand.GetAllAsync()).Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Candy obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\products");

                    if (!Directory.Exists(productPath))
                    {
                        Directory.CreateDirectory(productPath);
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    obj.ImageUrl = @"\images\products\" + fileName;
                }

                await _unitOfWork.Candy.AddAsync(obj);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Товар успешно создан";
                return RedirectToAction("Index");
            }

            ViewBag.CategoryList = (await _unitOfWork.Category.GetAllAsync()).Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            ViewBag.BrandList = (await _unitOfWork.Brand.GetAllAsync()).Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var candyFromDb = await _unitOfWork.Candy.GetAsync(id.Value);
            if (candyFromDb == null)
            {
                return NotFound();
            }

            ViewBag.CategoryList = (await _unitOfWork.Category.GetAllAsync()).Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            ViewBag.BrandList = (await _unitOfWork.Brand.GetAllAsync()).Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(candyFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Candy obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\products");

                    if (!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    obj.ImageUrl = @"\images\products\" + fileName;
                }

                await _unitOfWork.Candy.UpdateAsync(obj);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Товар успешно обновлен";
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
            var candyFromDb = await _unitOfWork.Candy.GetAsync(id.Value, includeProperties: "Category,Brand");
            if (candyFromDb == null)
            {
                return NotFound();
            }
            return View(candyFromDb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            var obj = await _unitOfWork.Candy.GetAsync(id.Value);
            if (obj == null)
            {
                return NotFound();
            }

            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (!string.IsNullOrEmpty(obj.ImageUrl))
            {
                var oldImagePath = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            await _unitOfWork.Candy.DeleteAsync(id.Value);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Товар успешно удален";
            return RedirectToAction("Index");
        }
    }
}