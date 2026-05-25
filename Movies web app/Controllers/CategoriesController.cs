using Business.DTOs.Categories;
using Business.Managers.Categories;
using Microsoft.AspNetCore.Mvc;

namespace Movies_web_app.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryManager _categoryManager;

        public CategoriesController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryManager.GetAllCategoriesAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryDto category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var newCategory = new CreateCategoryDto
            {
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl
            };

            await _categoryManager.CreateCategoryAsync(newCategory);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryManager.GetCategoryByIdAsync(id);
            if (category == null) return View("NotFound");

            var dto = new UpdateCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCategoryDto category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var newCategory = new UpdateCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl
            };

            await _categoryManager.UpdateCategoryAsync(newCategory);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryManager.GetCategoryByIdAsync(id);
            if (category == null) return View("NotFound");
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(UpdateCategoryDto category)
        {
            await _categoryManager.DeleteCategoryAsync(category.Id);
            return RedirectToAction("Index");
        }
    }
}
