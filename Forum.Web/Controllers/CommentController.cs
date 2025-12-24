using Forum.Data;
using Forum.Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Forum.Web.Controllers
{
    [Authorize] // Sadece giriş yapanlar yorum atabilir
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
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Kullanıcı yoksa JSON olarak hata dön
                return Json(new { success = false, message = "Oturum açmalısınız." });
            }

            comment.UserId = user.Id;
            comment.CreatedDate = DateTime.Now;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Sayfayı yenilemek yerine, eklenen veriyi JSON olarak geri yolluyoruz
            return Json(new
            {
                success = true,
                userName = user.Name ?? user.UserName, // Ad yoksa kullanıcı adı
                date = comment.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
                content = comment.Content
            });
        }
    }
}