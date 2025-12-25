using Forum.Data;
using Forum.Entity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    public class CommentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public CommentController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Comment comment)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Oturum açmanız gerekiyor." });
            }

            var user = await _userManager.GetUserAsync(User);

            // Yorum verilerini doldur
            comment.UserId = user.Id;
            comment.CreatedDate = DateTime.Now;

            // Kaydet
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // JSON Olarak Geri Dön (Sayfa yenilenmez, bu veriyi JavaScript yakalar)
            return Json(new
            {
                success = true,
                userName = user.UserName,
                // Kullanıcının resmi yoksa varsayılan avatar servisini kullan
                userImage = user.Image ?? "https://ui-avatars.com/api/?name=" + user.UserName + "&background=random",
                date = comment.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
                content = comment.Content
            });
        }
    }
}