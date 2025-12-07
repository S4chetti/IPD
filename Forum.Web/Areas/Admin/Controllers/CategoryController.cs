using Forum.Data;
using Forum.Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Area("Admin")]
[Authorize(Roles = "Admin")] // Sadece adminler girebilir
public class CategoryController : Controller
{
    private readonly IRepository<Category> _categoryRepo;

    public CategoryController(IRepository<Category> repo)
    {
        _categoryRepo = repo;
    }

    public IActionResult Index()
    {
        var categories = _categoryRepo.GetAll();
        return View(categories);
    }

    // Create, Edit, Delete actionları buraya eklenecek...
}