using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Models
{
    public class UserProfileViewModel
    {
        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Display(Name = "Soyad")]
        public string Surname { get; set; }
        public string? SurName { get; internal set; }
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Display(Name = "E-Posta")]
        public string Email { get; set; }

        [Display(Name = "Profil Resmi")]
        public string ImagePath { get; set; }

        // Dosya yükleme için gerekli alan
        [Display(Name = "Yeni Profil Resmi Yükle")]
        public IFormFile ImageFile { get; set; }
    }
}