using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Forum.Data;
using Forum.Entity.Models;

namespace Forum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // Bu kapıdan sadece Adminler geçebilir!
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _categoryRepo;

        public CategoryController(IRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        // 1. LİSTELEME SAYFASI
        public IActionResult Index()
        {
            var categories = _categoryRepo.GetAll();
            return View(categories);
        }

        // 2. EKLEME SAYFASI (Formu Göster)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 2. EKLEME İŞLEMİ (Veriyi Kaydet)
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _categoryRepo.Add(category);
            return RedirectToAction("Index"); // Listeye geri dön
        }

        // 3. SİLME İŞLEMİ
        public IActionResult Delete(int id)
        {
            _categoryRepo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}