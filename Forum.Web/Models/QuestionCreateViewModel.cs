using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Models
{
    public class QuestionCreateViewModel
    {
        [Required(ErrorMessage = "Başlık zorunludur.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Kategori seçmelisiniz.")]
        public int CategoryId { get; set; }

        // Dosya yükleme işlemi için bu tip kullanılır
        public IFormFile? Image { get; set; }
    }
}