using Microsoft.AspNetCore.Mvc;
using MyRecipe.Data;
using MyRecipe.Models;

namespace MyRecipe.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;

        public CategoryController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (_db.Categories.Any(x => x.Name == obj.Name))
            {
                TempData["ErrorMessage"] = "This category already exists";
                return View(obj);
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["SuccessMessage"] = $"Category: '{obj.Name}' has been added successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _db.Categories.Find(id);

            if (category == null) { return NotFound(); }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (_db.Categories.Any(x => x.Name == obj.Name))
            {
                TempData["ErrorMessage"] = "This category already exists";
                return View(obj);
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["SuccessMessage"] = $"Category: '{obj.Name}' has been changed successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var category = _db.Categories.Find(id);
            if (category == null) { return NotFound(); }
            _db.Categories.Remove(category);
            _db.SaveChanges();
            TempData["SuccessMessage"] = $"Category: '{category.Name}' has been deleted";
            return RedirectToAction("Index");
        }
    }
}
