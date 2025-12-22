using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    // [Authorize(Roles = "Admin")] // Bu satırı Rolleri ayarladığımızda açacağız
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}