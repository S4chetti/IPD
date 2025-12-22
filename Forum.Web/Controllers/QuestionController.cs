using Forum.Data;
using Forum.Entity.Models;
using Forum.Web.Models; // ViewModel'i buradan çekeceğiz
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO; // Dosya kaydetmek için şart!

namespace Forum.Web.Controllers
{
    [Authorize] // Sadece giriş yapmış kullanıcılar erişebilir
    public class QuestionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment; // <--- YENİ EKLENDİ

        // Constructor'a hostEnvironment parametresini ekliyoruz
        public QuestionController(AppDbContext context, UserManager<User> userManager, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment; // <--- YENİ EKLENDİ
        }

        // GET: Soru Ekleme Sayfasını Göster
        [HttpGet] // Sayfa ilk açıldığında çalışır
        public IActionResult Create()
        {
            // Kategorileri Dropdown (Açılır Liste) için hazırlıyoruz
            // "Id" veritabanına gidecek değer, "Name" kullanıcının göreceği değer
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost] // Form gönderildiğinde çalışır
        [ValidateAntiForgeryToken] // Güvenlik önlemi
        public async Task<IActionResult> Create(QuestionCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                // ADIM 1: RESİM VAR MI KONTROL ET
                if (model.Image != null)
                {
                    // Resimlerin kaydedileceği ana klasör: wwwroot/img/questions
                    // WebRootPath bize 'wwwroot' klasörünün tam yolunu verir.
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "img", "questions");

                    // Eğer klasör yoksa oluştur (Hata almamak için)
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Dosya adı çakışmasın diye rastgele bir GUID ekliyoruz.
                    // Örn: "deneme.jpg" -> "b123-asda-2131_deneme.jpg" olur.
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;

                    // Tam dosya yolu (C:/Projects/.../wwwroot/img/questions/....jpg)
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Resmi sunucuya kopyala (Kaydet)
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(fileStream);
                    }
                }

                // ADIM 2: KULLANICIYI BUL
                var user = await _userManager.GetUserAsync(User);

                // ADIM 3: VERİTABANI NESNESİNİ OLUŞTUR
                // ViewModel'deki verileri gerçek Entity'ye aktarıyoruz.
                Question newQuestion = new Question
                {
                    Title = model.Title,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    UserId = user.Id,
                    CreatedDate = DateTime.Now,
                    ImageName = uniqueFileName // Resmin sadece adını veritabanına yazıyoruz!
                };

                // ADIM 4: KAYDET
                _context.Questions.Add(newQuestion);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            // Eğer hata varsa (örneğin başlık boşsa), kategorileri tekrar yükle ve formu geri göster
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View(model);
        }
    }
}
