using Forum.Data.Abstract; // Bunu eklemeyi unutma
using Forum.Data.Concrete;
using Forum.Entity.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Forum.Web.Controllers
{
    public class HomeController : Controller
    {
        // ARTIK GENERIC DEGIL, OZEL REPOSITORY KULLANIYORUZ

private readonly IQuestionRepository _questionRepository; // <-- DOĞRU

    public HomeController(IQuestionRepository questionRepository) // <-- DOĞRU
    {
        _questionRepository = questionRepository;
    }

        public IActionResult Index()
        {
            return View(_questionRepository.GetAllWithDetails());
        }

        public IActionResult Details(int id)
        {
            // Eski Kod: var question = _questionRepository.GetById(id); (YETERSİZ)

            // YENİ KOD: (Yazarı, Yorumları, Resmi her şeyi getirir)
            var question = _questionRepository.GetQuestionWithDetails(id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }
    }
}