using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Forum.Entity.Models
{
    // IdentityUser<int> ile Identity özelliklerini (Password, Email vb.) alıyoruz.
    public class User : IdentityUser<int>
    {
        public string? Name { get; set; }
        public string? SurName { get; set; }

        // --- İŞTE EKSİK OLAN SATIR BURASI ---
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        // ------------------------------------

        public string? ImageId { get; set; }

        // İlişkiler
        public List<Question> Questions { get; set; }
        public List<Comment> Comments { get; set; }
    }
}