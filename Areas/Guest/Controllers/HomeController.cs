using Microsoft.AspNetCore.Mvc;
using MyRecipe.DataAccess.Repository.IRepository;
using MyRecipe.Models;
using MyRecipe.Models.ViewModels;
using System.Diagnostics;

namespace MyRecipe.Areas.Guest.Controllers;
[Area("Guest")]

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Recipe> productList = _unitOfWork.Recipe.GetAll(includeProperties: "Category,Difficulty");

        return View(productList.OrderByDescending(d => d.Created));
    }
    public IActionResult Details(int id)
    {
        RecipeVM recipeVM = new()
        {
            Recipe = _unitOfWork.Recipe.GetFirstOrDefault(u => u.Id == id, includeProperties: "Category,Difficulty")
        };
        return View(recipeVM);
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
