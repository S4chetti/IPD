using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Forum.Data;
using Forum.Entity.Models;
using Forum.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace Forum.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository<User> _userRepo;

        public AccountController(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = _userRepo.GetAll().FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("UserId", user.Id.ToString())
                };

                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("CookieAuth", principal);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "E-posta veya şifre hatalı.";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 1. Bu e-posta ile daha önce kayıt olunmuş mu?
            var existingUser = _userRepo.GetAll().FirstOrDefault(x => x.Email == model.Email);
            if (existingUser != null)
            {
                ViewBag.Error = "Bu e-posta adresi zaten kullanılıyor.";
                return View(model);
            }

            // 2. Yeni kullanıcıyı oluştur
            var newUser = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password, // Gerçek projelerde şifreler hashlenmelidir!
                Role = "User",
                CreatedDate = DateTime.Now
            };

            // 3. Veritabanına kaydet
            _userRepo.Add(newUser);

            // 4. Giriş sayfasına yönlendir
            return RedirectToAction("Login");
        }
    }
}