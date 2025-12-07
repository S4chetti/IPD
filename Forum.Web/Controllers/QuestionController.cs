using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Giriş kontrolü için
using System.Security.Claims; // Kullanıcı ID'sini almak için
using Forum.Data;
using Forum.Entity.Models;
using Microsoft.AspNetCore.Mvc.Rendering; // Dropdown listesi için

namespace Forum.Web.Controllers
{
    [Authorize] // Bu controller'daki işlemleri sadece giriş yapanlar görebilir
    public class QuestionController : Controller
    {
        private readonly IRepository<Question> _questionRepo;
        private readonly IRepository<Category> _categoryRepo;

        public QuestionController(IRepository<Question> questionRepo, IRepository<Category> categoryRepo)
        {
            _questionRepo = questionRepo;
            _categoryRepo = categoryRepo;
        }

        // GET: Soru Ekleme Sayfasını Göster
        [HttpGet]
        public IActionResult Create()
        {
            // Kategorileri Dropdown (Açılır Liste) için hazırla
            ViewBag.Categories = new SelectList(_categoryRepo.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Formdan gelen veriyi kaydet
        [HttpPost]
        public IActionResult Create(Question question)
        {
            // Kullanıcı ID'sini otomatik al (Formdan gelmez, güvenlik için buradan alırız)
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim != null)
            {
                question.UserId = int.Parse(userIdClaim.Value);
            }

            question.CreatedAt = DateTime.Now;

            // Basit doğrulama (Kategori seçilmiş mi, başlık var mı?)
            if (string.IsNullOrEmpty(question.Title) || string.IsNullOrEmpty(question.Description))
            {
                ViewBag.Error = "Lütfen başlık ve açıklama giriniz.";
                ViewBag.Categories = new SelectList(_categoryRepo.GetAll(), "Id", "Name");
                return View(question);
            }

            _questionRepo.Add(question);
            return RedirectToAction("Index", "Home"); // Başarılıysa ana sayfaya git
        }
    }
}