using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyRecipe.DataAccess.Repository.IRepository;
using MyRecipe.Models.ViewModels;
using MyRecipe.Utility;
using System.Security.Claims;

namespace MyRecipe.Areas.User.Controllers;
[Area("User")]
[Authorize(Roles = SD.Role_User + "," + SD.Role_Admin)]


public class RecipeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly UserManager<IdentityUser> _userManager;


    public RecipeController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        RecipeVM recipeVM = new()
        {
            Recipe = new(),
            CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            }),
            DifficultyList = _unitOfWork.Difficulty.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            }),

        };

        return View(recipeVM);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(RecipeVM obj, IFormFile file)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        obj.Recipe.ApplicationUserId = claim.Value;
        obj.Recipe.ApplicationUser = User.Identity.Name;


        string wwwRootPath = _webHostEnvironment.WebRootPath;
        if (file != null)
        {
            string fileName = Guid.NewGuid().ToString();
            var uploads = Path.Combine(wwwRootPath, @"images\recipes");
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (obj.Recipe.ImageUrl != null)
            {
                var oldImagePath = Path.Combine(wwwRootPath, obj.Recipe.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
            {
                file.CopyTo(fileStreams);
            }
            obj.Recipe.ImageUrl = @"\images\recipes\" + fileName + extension;
        }
        obj.Recipe.Created = DateTime.Now;

        _unitOfWork.Recipe.Add(obj.Recipe);
        _unitOfWork.Save();
        TempData["SuccessMessage"] = $"Recipe: '{obj.Recipe.Title}' has been added successfully";
        return RedirectToAction("Index", "Home", new { area = "Guest" });

    }


}


