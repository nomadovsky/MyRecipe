using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRecipe.DataAccess.Repository.IRepository;
using MyRecipe.Models;
using MyRecipe.Utility;

namespace MyRecipe.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]

public class DifficultyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public DifficultyController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        IEnumerable<Difficulty> objDifficultyList = _unitOfWork.Difficulty.GetAll();
        return View(objDifficultyList);
    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Difficulty obj)
    {
        if (_unitOfWork.Difficulty.GetFirstOrDefault(x => x.Name == obj.Name) != null)
        {
            TempData["ErrorMessage"] = "This category already exists";
            return View(obj);
        }
        if (ModelState.IsValid)
        {
            _unitOfWork.Difficulty.Add(obj);
            _unitOfWork.Save();
            TempData["SuccessMessage"] = $"Difficulty: '{obj.Name}' has been added successfully";
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
        var category = _unitOfWork.Difficulty.GetFirstOrDefault(u => u.Id == id);


        if (category == null) { return NotFound(); }
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Difficulty obj)
    {
        if (_unitOfWork.Difficulty.GetFirstOrDefault(x => x.Name == obj.Name) != null)
        {
            TempData["ErrorMessage"] = "This category already exists";
            return View(obj);
        }
        if (ModelState.IsValid)
        {
            _unitOfWork.Difficulty.Update(obj);
            _unitOfWork.Save();
            TempData["SuccessMessage"] = $"Difficulty: '{obj.Name}' has been changed successfully";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var category = _unitOfWork.Difficulty.GetFirstOrDefault(u => u.Id == id);
        if (category == null) { return NotFound(); }
        _unitOfWork.Difficulty.Remove(category);
        _unitOfWork.Save();
        TempData["SuccessMessage"] = $"Difficulty: '{category.Name}' has been deleted";
        return RedirectToAction("Index");
    }
}

