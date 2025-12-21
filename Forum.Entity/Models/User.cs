using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Forum.Entity.Models
{
    // IdentityUser<int> diyerek ID'nin integer kalmasını sağlıyoruz (varsayılan string GUID'dir)
    public class User : IdentityUser<int>
    {
        // IdentityUser'da olmayan kendi özel alanların:
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;

        // Profil resmi için dosya yolu
        public string? ImageId { get; set; }

        // İlişkiler
        public List<Question> Questions { get; set; }
        public List<Comment> Comments { get; set; }
    }
}