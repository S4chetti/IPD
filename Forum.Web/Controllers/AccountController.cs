using Forum.Entity.Models;
using Forum.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Forum.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

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
            //sd
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

        private readonly IWebHostEnvironment _webHostEnvironment;

        // Constructor'a (Yapıcı Metoda) IWebHostEnvironment ekliyoruz (Resim kaydetmek için)
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var model = new UserProfileViewModel
            {
                Name = user.Name,
                SurName = user.SurName,
                UserName = user.UserName,
                Email = user.Email,
                ImagePath = user.ImagePath // User tablosunda ImagePath alanı olduğunu varsayıyoruz
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                // 1. Bilgileri Güncelle
                user.Name = model.Name;
                user.SurName = model.Surname;
                // Email ve Username değiştirmek istersen buraya ekleyebilirsin ama genelde ayrı süreç ister.

                // 2. Resim Yükleme İşlemi
                if (model.ImageFile != null)
                {
                    // Resim uzantısını al (örn: .jpg)
                    var extension = Path.GetExtension(model.ImageFile.FileName);
                    // Benzersiz bir isim oluştur (Guid)
                    var newImageName = Guid.NewGuid() + extension;
                    // Kaydedilecek klasör yolu (wwwroot/img/users)
                    var location = Path.Combine(_webHostEnvironment.WebRootPath, "img/users");

                    // Klasör yoksa oluştur
                    if (!Directory.Exists(location))
                        Directory.CreateDirectory(location);

                    var path = Path.Combine(location, newImageName);

                    // Resmi kaydet
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    // Eski resmi silmek istersen burada silebilirsin (isteğe bağlı)

                    // Veritabanına resim yolunu kaydet
                    user.ImagePath = "/img/users/" + newImageName;
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["Message"] = "Profiliniz başarıyla güncellendi.";
                    return RedirectToAction("Profile");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
    }
}