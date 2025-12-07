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
            var questions = _questionRepo.GetAll();
            return View(questions);
        }

        public IActionResult Details(int id)
        {
            var question = _questionRepo.GetAll().FirstOrDefault(x => x.Id == id);
            return View(question);
        }

        [HttpPost]
        public IActionResult AddComment(string content, int questionId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Lütfen giriş yapınız." });
            }

            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return Json(new { success = false, message = "Kullanıcı ID bulunamadı." });
            }

            var userId = int.Parse(userIdClaim.Value);

            var comment = new Comment
            {
                Content = content,
                QuestionId = questionId,
                UserId = userId,
                CreatedAt = DateTime.Now
            };

            _commentRepo.Add(comment);
            return Json(new { success = true });
        }
    }
}