using Microsoft.AspNetCore.Mvc;
using MyRecipe.DataAccess.Repository.IRepository;
using MyRecipe.Models;

namespace MyRecipe.Areas.Admin.Controllers;
[Area("Admin")]


public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
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
        if (_unitOfWork.Category.GetFirstOrDefault(x => x.Name == obj.Name) != null)
        {
            TempData["ErrorMessage"] = "This category already exists";
            return View(obj);
        }
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Add(obj);
            _unitOfWork.Save();
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
        //var category = _unitOfWork.Category.Categories.Find(id);
        var category = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);


        if (category == null) { return NotFound(); }
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category obj)
    {
        if (_unitOfWork.Category.GetFirstOrDefault(x => x.Name == obj.Name) != null)
        {
            TempData["ErrorMessage"] = "This category already exists";
            return View(obj);
        }
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Update(obj);
            _unitOfWork.Save();
            TempData["SuccessMessage"] = $"Category: '{obj.Name}' has been changed successfully";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var category = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
        if (category == null) { return NotFound(); }
        _unitOfWork.Category.Remove(category);
        _unitOfWork.Save();
        TempData["SuccessMessage"] = $"Category: '{category.Name}' has been deleted";
        return RedirectToAction("Index");
    }
}
