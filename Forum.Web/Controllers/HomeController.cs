using Microsoft.AspNetCore.Mvc;
using Forum.Data;
using Forum.Entity.Models;
using Forum.Web.Models;
using System.Security.Claims;

namespace Forum.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Question> _questionRepo;
        private readonly IRepository<Comment> _commentRepo;

        public HomeController(IRepository<Question> questionRepo, IRepository<Comment> commentRepo)
        {
            _questionRepo = questionRepo;
            _commentRepo = commentRepo;
        }

        public IActionResult Index()
        {
            // BURASI DEĞİŞTİ: "Category" tablosunu da getir diyoruz.
            var questions = _questionRepo.GetAll("Category,User,Comments"); 
            return View(questions);
        }

        public IActionResult Details(int id)
        {
            // Burası çok önemli! Zincirleme veri çekiyoruz:
            // 1. Category -> Kategorisini getir.
            // 2. User -> Soruyu yazanı getir.
            // 3. Comments -> Yorumları getir.
            // 4. Comments.User -> Yorumu yazan kişileri de getir (İç içe include).
            var question = _questionRepo.GetAll("Category,User,Comments,Comments.User")
                                        .FirstOrDefault(x => x.Id == id);

            return View(question);
        }

        [HttpPost]
        public IActionResult AddComment(string content, int questionId)
        {
            // 1. Kullanıcı giriş yapmış mı kontrol et
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Lütfen önce giriş yapınız." });
            }

            // 2. Kullanıcı ID'sini bul
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return Json(new { success = false, message = "Kullanıcı bilgisi alınamadı." });
            }
            int userId = int.Parse(userIdClaim.Value);

            // 3. Yorumu oluştur
            var comment = new Comment
            {
                Content = content,
                QuestionId = questionId,
                UserId = userId,
                CreatedAt = DateTime.Now
            };

            // 4. Veritabanına kaydet
            _commentRepo.Add(comment);

            // 5. Frontend'e JSON formatında "Başarılı" mesajı ve eklenen veriyi dön
            // (Eklenen yorumun HTML'ini JS tarafında oluşturacağız)
            // Veritabanına kaydettikten sonra ID oluşur, onu da gönderiyoruz:
            return Json(new { success = true, commentId = comment.Id });
        }

        // Yorum Silme İşlemi
        public IActionResult DeleteComment(int id)
        {
            // 1. Silinecek yorumu bul
            var comment = _commentRepo.GetById(id);

            if (comment == null)
            {
                return NotFound();
            }

            // 2. Güvenlik Kontrolü: Şu anki kullanıcı bu yorumun sahibi mi?
            // (Admin ise her şeyi silebilir)
            var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            if (comment.UserId != currentUserId && !User.IsInRole("Admin"))
            {
                // Yetkisiz işlem denemesi!
                return RedirectToAction("Details", new { id = comment.QuestionId });
            }

            // 3. Sil ve geri dön
            _commentRepo.Delete(id);

            return RedirectToAction("Details", new { id = comment.QuestionId });
        }
    }
}