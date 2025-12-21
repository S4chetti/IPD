using Forum.Entity.Models;
using Forum.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 1. Önce kullanıcıyı Email ile bul
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Bu email adresiyle kayıtlı kullanıcı bulunamadı.");
                return View(model);
            }

            // 2. Kullanıcı bulunduysa şifreyi kontrol et
            // user.UserName parametresini veriyoruz çünkü SignInManager UserName bekler
            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            // 3. Başarısız olursa hata mesajı ekle
            ModelState.AddModelError("", "Email veya şifre hatalı.");
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new User()
            {
                Name = model.Name,
                SurName = model.SurName,
                UserName = model.Email, // Identity'de UserName zorunludur, Email'i kullanabiliriz
                Email = model.Email,
                CreatedDate = DateTime.Now,
                ImageId = "default-profile.png" // Varsayılan resim
            };

            // CreateAsync metodu şifreyi otomatik hashler ve kaydeder
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Kayıt başarılıysa otomatik giriş yap
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            // Hata varsa ekrana bas (Örn: Şifre çok kısa)
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}