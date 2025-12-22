using Forum.Data.Abstract; // Bunu ekle
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    public class HomeController : Controller
    {
        // Service yerine doğrudan Repository kullanıyoruz (Daha pratik)
        private readonly IQuestionRepository _questionRepo;

        public HomeController(IQuestionRepository questionRepo)
        {
            _questionRepo = questionRepo;
        }

        public IActionResult Index()
        {
            // Anasayfada tüm sorular
            return View(_questionRepo.GetAll());
        }

        public IActionResult Details(int id)
        {
            // --- HATA VEREN KISIM BURAYDI, ARTIK ÇALIŞACAK ---
            var question = _questionRepo.GetQuestionWithDetails(id);
            // -------------------------------------------------

            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }
    }
}